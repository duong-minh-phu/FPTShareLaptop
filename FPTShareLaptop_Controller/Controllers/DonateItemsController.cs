using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccess.DonateItemDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.IService;
using Service.Service;
using System.Text.RegularExpressions;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/donate-items")]
    [ApiController]
    public class DonateItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly OpenAIService _openAIService;

        public DonateItemsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary, OpenAIService openAIService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _openAIService = openAIService;
        }

        // GET: api/donate-items
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _unitOfWork.DonateItem.GetAllAsync();
            var itemDTOs = _mapper.Map<IEnumerable<DonateItemReadDTO>>(items);
            return Ok(ResultModel.Success(itemDTOs));
        }

        // GET: api/donate-items/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ResultModel.NotFound());
            }
            var itemDTO = _mapper.Map<DonateItemReadDTO>(item);
            return Ok(ResultModel.Success(itemDTO));
        }

        // POST: api/donate-items (Tạo mới + upload ảnh)
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] DonateItemCreateDTO itemDTO)
        {
            if (itemDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }
            var donateForm = await _unitOfWork.DonateForm.GetByIdAsync(itemDTO.DonateFormId);
            if (donateForm == null)
            {
                return BadRequest(ResultModel.BadRequest("DonateFormId không tồn tại."));
            }

            // Upload ảnh lên Cloudinary
            string imageUrl = null;
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "donate_item_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var item = _mapper.Map<DonateItem>(itemDTO);
            item.ItemImage = imageUrl;
            item.Status = "Available";
            item.CreatedDate = DateTime.UtcNow;
            item.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.DonateItem.AddAsync(item);

            donateForm.Status = "Done";
            _unitOfWork.DonateForm.Update(donateForm);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<DonateItemReadDTO>(item);
            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, ResultModel.Created(result));
        }

        // PUT: api/donate-items/{id} (Cập nhật thông tin + upload ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonateItem(int id, IFormFile? file, [FromForm] DonateItemUpdateDTO dto)
        {
            var existingItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound(ResultModel.NotFound("Không tìm thấy DonateItem."));
            }

            // Upload ảnh nếu có file mới
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "donate_item_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                existingItem.ItemImage = uploadResult.SecureUrl.ToString();
            }

            // Cập nhật các thuộc tính có giá trị
            if (!string.IsNullOrEmpty(dto.ItemName)) existingItem.ItemName = dto.ItemName;
            if (!string.IsNullOrEmpty(dto.Cpu)) existingItem.Cpu = dto.Cpu;
            if (!string.IsNullOrEmpty(dto.Ram)) existingItem.Ram = dto.Ram;
            if (!string.IsNullOrEmpty(dto.Storage)) existingItem.Storage = dto.Storage;
            if (!string.IsNullOrEmpty(dto.ScreenSize)) existingItem.ScreenSize = dto.ScreenSize;
            if (!string.IsNullOrEmpty(dto.ConditionItem)) existingItem.ConditionItem = dto.ConditionItem;
            if (!string.IsNullOrEmpty(dto.Status)) existingItem.Status = dto.Status;
            if (!string.IsNullOrEmpty(dto.SerialNumber)) existingItem.SerialNumber = dto.SerialNumber;
            if (!string.IsNullOrEmpty(dto.Model)) existingItem.Model = dto.Model;
            if (!string.IsNullOrEmpty(dto.Color)) existingItem.Color = dto.Color;
            if (!string.IsNullOrEmpty(dto.GraphicsCard)) existingItem.GraphicsCard = dto.GraphicsCard;
            if (!string.IsNullOrEmpty(dto.Battery)) existingItem.Battery = dto.Battery;
            if (!string.IsNullOrEmpty(dto.Ports)) existingItem.Ports = dto.Ports;
            if (dto.ProductionYear.HasValue) existingItem.ProductionYear = dto.ProductionYear.Value;
            if (!string.IsNullOrEmpty(dto.OperatingSystem)) existingItem.OperatingSystem = dto.OperatingSystem;
            if (!string.IsNullOrEmpty(dto.Description)) existingItem.Description = dto.Description;
            if (dto.CategoryId.HasValue) existingItem.CategoryId = dto.CategoryId.Value;

            // Cập nhật ngày sửa
            existingItem.UpdatedDate = DateTime.UtcNow;

            // Lưu thay đổi
            _unitOfWork.DonateItem.Update(existingItem);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Cập nhật thành công"));
        }


        // DELETE: api/donate-items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ResultModel.NotFound());
            }

            _unitOfWork.DonateItem.Delete(item);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }

        // Lấy đề xuất laptop theo ngành học
        
        [HttpGet("suggest-laptops")]
        public async Task<IActionResult> SuggestLaptops(string major)
        {
            var allItems = (await _unitOfWork.DonateItem.GetAllAsync()).ToList();
            var result = await _openAIService.GetLaptopSuggestionAsync(major, allItems);
            return Ok(ResultModel.Success(result, "Gợi ý cấu hình thành công."));
        }

        private bool CompareCpu(string laptopCpu, string requestedCpu)
        {
            // Extract the CPU series number from both the laptop and requested CPU, e.g., i5, i7, i9
            var laptopCpuSeries = GetCpuSeries(laptopCpu);
            var requestedCpuSeries = GetCpuSeries(requestedCpu);

            // Compare CPU series (i9 > i7 > i5 > i3)
            return laptopCpuSeries >= requestedCpuSeries;
        }

        private bool CompareRam(string laptopRam, int requestedRam)
        {
            // Extract the RAM size and compare
            var laptopRamSize = ExtractRamSize(laptopRam);
            return laptopRamSize >= requestedRam;
        }

        private int GetCpuSeries(string cpu)
        {
            // Convert the CPU string (e.g., "Intel Core i5") into a numerical value (e.g., 5 for i5)
            if (cpu.Contains("i7")) return 7;
            if (cpu.Contains("i5")) return 5;
            if (cpu.Contains("i3")) return 3;
            if (cpu.Contains("i9")) return 9;
            return 0; // Default case
        }

        private int ExtractRamSize(string ram)
        {
            // Extract the RAM size from the string (e.g., "16GB" -> 16)
            if (int.TryParse(new string(ram.Where(char.IsDigit).ToArray()), out var ramSize))
            {
                return ramSize;
            }
            return 0;
        }

        private (string Cpu, int Ram) ParseMajor(string major)
        {
            // Giả sử 'major' chứa thông tin về CPU và RAM
            var cpuRegex = new Regex(@"(Intel Core [i7|i5|i3|i9]|AMD Ryzen \d+)");
            var ramRegex = new Regex(@"\d+GB RAM");

            var cpuMatch = cpuRegex.Match(major);
            var ramMatch = ramRegex.Match(major);

            string cpu = cpuMatch.Success ? cpuMatch.Value : string.Empty;
            int ram = ramMatch.Success ? int.Parse(new string(ramMatch.Value.Where(char.IsDigit).ToArray())) : 0;

            return (cpu, ram);
        }

        [HttpGet("top-donors")]
        public async Task<IActionResult> GetTopDonors()
        {
            var items = await _unitOfWork.DonateItem.GetAllAsync(includeProperties: x => x.User);

            var topDonors = items
                .Where(x => x.Status != "Deleted") // Nếu bạn lọc như vậy
                .GroupBy(item => new { item.UserId, item.User.FullName })
                .Select(group => new TopDonorDTO
                {
                    DonorId = group.Key.UserId,
                    DonorName = group.Key.FullName,
                    TotalLaptops = group.Count()
                })
                .OrderByDescending(dto => dto.TotalLaptops)
                .Take(5)
                .ToList();

            return Ok(ResultModel.Success(topDonors, "Top 5 người donate nhiều laptop nhất"));
        }


        // GET: api/donate-items/borrowed
        [HttpGet("borrowed")]
        public async Task<IActionResult> GetBorrowedItems()
        {
            var contracts = await _unitOfWork.BorrowContract.GetAllAsync(
                includeProperties: x => x.Item
            );

            var items = contracts
                .Where(x => x.Item != null)
                .Select(x => x.Item)
                .Distinct()
                .ToList();

            var itemDTOs = _mapper.Map<IEnumerable<DonateItemReadDTO>>(items);
            return Ok(ResultModel.Success(itemDTOs, "Danh sách laptop đã có hợp đồng"));
        }

        // GET: api/donate-items/approved
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedBorrowItems()
        {
            var approvedRequests = await _unitOfWork.BorrowRequest.GetAllAsync(
                filter: x => x.Status == "Approved",
                includeProperties: x => x.Item
            );

            // Lấy ra danh sách DonateItem từ các BorrowRequest đã Approved
            var approvedItems = approvedRequests
                .Where(x => x.Item != null)
                .Select(x => x.Item)
                .Distinct() // Nếu một item được duyệt nhiều lần thì chỉ lấy 1 lần
                .ToList();

            var approvedItemDTOs = _mapper.Map<IEnumerable<DonateItemReadDTO>>(approvedItems);
            return Ok(ResultModel.Success(approvedItemDTOs, "Danh sách laptop đã được duyệt mượn"));
        }


    }

}
