﻿using AutoMapper;
using BusinessObjects.Models;
using DataAccess.DonateItemDTO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Service
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;  // Lưu API Key vào một biến
        private static Dictionary<string, (string result, DateTime createdAt)> _cache = new();
        private static int _dailyRequestCount = 0;
        private static DateTime _lastReset = DateTime.UtcNow.Date;
        private readonly IMapper _mapper;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _cache = new Dictionary<string, (string, DateTime)>();
            _dailyRequestCount = 0;
            _lastReset = DateTime.UtcNow.Date;

            // Lấy API Key từ biến môi trường
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("OpenAI API key is not set in environment variables.");
            }
        }

        public async Task<LaptopSuggestionResultDTO> GetLaptopSuggestionAsync(string major, List<DonateItem> donateItems)
        {
            if (_lastReset != DateTime.UtcNow.Date)
            {
                _dailyRequestCount = 0;
                _lastReset = DateTime.UtcNow.Date;
            }

            if (_dailyRequestCount >= 30)
                throw new Exception("Bạn đã vượt quá số request tối đa trong ngày (30).");

            if (_cache.ContainsKey(major) && DateTime.UtcNow - _cache[major].createdAt < TimeSpan.FromDays(1))
            {
                return System.Text.Json.JsonSerializer.Deserialize<LaptopSuggestionResultDTO>(_cache[major].result)!;
            }

            var url = "https://api.openai.com/v1/chat/completions";

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
                new { role = "system", content = "Bạn là một chuyên gia tư vấn laptop." },
                new { role = "user", content = $"Tôi học ngành {major}, hãy gợi ý cấu hình laptop tập trung vào CPU và RAM." }
            },
                temperature = 0.7
            };

            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenAI Error: {responseContent}");

            var resultJson = JsonDocument.Parse(responseContent);
            var message = resultJson.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            var cpuRegex = new Regex(@"Intel Core i\d", RegexOptions.IgnoreCase);
            var ramRegex = new Regex(@"\d+\s*GB\s*RAM", RegexOptions.IgnoreCase);

            var cpuMatch = cpuRegex.Match(message);
            var ramMatch = ramRegex.Match(message);

            string cpu = cpuMatch.Success ? cpuMatch.Value : "Intel Core i";
            string ram = ramMatch.Success ? ramMatch.Value : "Không tìm thấy RAM phù hợp";

            string suggestion = $"Đề xuất cấu hình: CPU: {cpu}, RAM: {ram}";

            int ExtractCpuLevel(string cpuString)
            {
                var match = Regex.Match(cpuString, @"i(\d)", RegexOptions.IgnoreCase);
                return match.Success ? int.Parse(match.Groups[1].Value) : 0;
            }

            int ExtractRamNumber(string ramString)
            {
                var match = Regex.Match(ramString, @"\d+");
                return match.Success ? int.Parse(match.Value) : 0;
            }

            int requiredCpuLevel = ExtractCpuLevel(cpu);
            int requiredRam = ExtractRamNumber(ram);

            var suitableItems = donateItems.Where(item =>
            {
                int itemCpuLevel = ExtractCpuLevel(item.Cpu ?? "");
                int itemRam = ExtractRamNumber(item.Ram ?? "");

                bool isCpuOk = itemCpuLevel >= requiredCpuLevel;
                bool isRamOk = requiredRam == 0 || itemRam >= requiredRam;

                return isCpuOk && isRamOk;
            }).ToList();

            var suitableDtos = _mapper.Map<List<DonateItemReadDTO>>(suitableItems);

            var result = new LaptopSuggestionResultDTO
            {
                SuggestionMessage = suggestion,
                SuitableLaptops = suitableDtos
            };

            _cache[major] = (System.Text.Json.JsonSerializer.Serialize(result), DateTime.UtcNow);
            _dailyRequestCount++;

            return result;
        }
    }
}
