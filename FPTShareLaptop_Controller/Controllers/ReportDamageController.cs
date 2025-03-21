using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ReportDamageDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDamageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public ReportDamageController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/ReportDamage
        [HttpGet]
        public async Task<IActionResult> GetReportDamages()
        {
            var reports = await _unitOfWork.ReportDamage.GetAllAsync();
            var reportDTOs = _mapper.Map<IEnumerable<ReportDamageDTO>>(reports);
            return Ok(reportDTOs);
        }

        // GET: api/ReportDamage/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportDamage(int id)
        {
            var report = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            var reportDTO = _mapper.Map<ReportDamageDTO>(report);
            return Ok(reportDTO);
        }

        // POST: api/ReportDamage (Tạo mới + upload ảnh)
        [HttpPost]
        public async Task<IActionResult> CreateReportDamage( IFormFile file, [FromForm] ReportDamageCreateDTO reportDTO)
        {
            if (reportDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            // Upload ảnh lên Cloudinary
            string imageUrl = null;
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "report_damage_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var report = _mapper.Map<ReportDamage>(reportDTO);
            report.ImageUrlreport = imageUrl; // Lưu URL ảnh vào DB
            report.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.ReportDamage.AddAsync(report);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetReportDamage), new { id = report.ReportId }, _mapper.Map<ReportDamageDTO>(report));
        }

        // PUT: api/ReportDamage/{id} (Cập nhật thông tin + upload ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReportDamage(int id,  IFormFile? file, [FromForm] ReportDamageUpdateDTO reportDTO)
        {
            if (reportDTO == null || reportDTO.ReportId != id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingReport = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            if (existingReport == null)
            {
                return NotFound();
            }

            // Upload ảnh mới lên Cloudinary nếu có file mới
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "report_damage_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                existingReport.ImageUrlreport = uploadResult.SecureUrl.ToString(); // Cập nhật URL ảnh mới
            }

            _mapper.Map(reportDTO, existingReport);
            existingReport.CreatedDate = DateTime.UtcNow;

            _unitOfWork.ReportDamage.Update(existingReport);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/ReportDamage/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportDamage(int id)
        {
            var report = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _unitOfWork.ReportDamage.Delete(report);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
