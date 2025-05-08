using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ReportDamageDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/report-damages")]
    [ApiController]
    public class ReportDamagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public ReportDamagesController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reports = await _unitOfWork.ReportDamage.GetAllAsync();
            var reportDTOs = _mapper.Map<IEnumerable<ReportDamageDTO>>(reports);
            return Ok(ResultModel.Success(reportDTOs, "Fetched all reports successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var report = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            return report != null
                ? Ok(ResultModel.Success(_mapper.Map<ReportDamageDTO>(report), "Fetched report successfully."))
                : NotFound(ResultModel.NotFound("Report not found."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(IFormFile file, [FromForm] ReportDamageCreateDTO reportDTO)
        {
            if (reportDTO == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

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
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var report = _mapper.Map<ReportDamage>(reportDTO);
            report.ImageUrlreport = imageUrl;
            report.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.ReportDamage.AddAsync(report);
            await _unitOfWork.SaveAsync();

            return StatusCode(201, ResultModel.Created(_mapper.Map<ReportDamageDTO>(report), "Report created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, IFormFile? file, [FromForm] ReportDamageUpdateDTO reportDTO)
        {
            if (reportDTO == null || reportDTO.ReportId != id)
                return BadRequest(ResultModel.BadRequest("ID mismatch."));

            var existingReport = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            if (existingReport == null)
                return NotFound(ResultModel.NotFound("Report not found."));

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
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                existingReport.ImageUrlreport = uploadResult.SecureUrl.ToString();
            }

            _mapper.Map(reportDTO, existingReport);
            existingReport.CreatedDate = DateTime.UtcNow;

            _unitOfWork.ReportDamage.Update(existingReport);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Report updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var report = await _unitOfWork.ReportDamage.GetByIdAsync(id);
            if (report == null)
                return NotFound(ResultModel.NotFound("Report not found."));

            _unitOfWork.ReportDamage.Delete(report);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Report deleted successfully."));
        }
    }
}
