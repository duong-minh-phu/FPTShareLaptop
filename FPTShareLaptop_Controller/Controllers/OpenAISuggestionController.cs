using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/openai")]
    [ApiController]
    public class OpenAISuggestionController : ControllerBase
    {
        private readonly OpenAIService _openAIService;

        public OpenAISuggestionController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpGet("suggest-laptop")]
        public async Task<IActionResult> SuggestLaptop([FromQuery] string major)
        {
            if (string.IsNullOrWhiteSpace(major))
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    code = 400,
                    message = "Tham số 'major' không hợp lệ."
                });
            }

            try
            {
                // Gọi OpenAI để lấy cấu hình đề xuất (CPU, RAM)
                var result = await _openAIService.GetLaptopSuggestionAsync(major, new List<DonateItem>());

                return Ok(new
                {
                    isSuccess = true,
                    code = 200,
                    data = result,
                    message = "Gợi ý cấu hình thành công."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    isSuccess = false,
                    code = 500,
                    message = $"Lỗi khi gọi OpenAI: {ex.Message}"
                });
            }
        }
    }
}
