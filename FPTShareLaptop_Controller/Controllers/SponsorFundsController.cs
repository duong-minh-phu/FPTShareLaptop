using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ResultModel;
using DataAccess.SponsorFundDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/sponsor-funds")]
    [ApiController]
    public class SponsorFundsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public SponsorFundsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var funds = await _unitOfWork.SponsorFund.GetAllAsync();
            var fundDTOs = _mapper.Map<IEnumerable<SponsorFundReadDTO>>(funds);
            return Ok(ResultModel.Success(fundDTOs));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fund = await _unitOfWork.SponsorFund.GetByIdAsync(id);
            if (fund == null)
                return NotFound(ResultModel.NotFound());

            var fundDTO = _mapper.Map<SponsorFundReadDTO>(fund);
            return Ok(ResultModel.Success(fundDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile? file, [FromForm] SponsorFundCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            string? imageUrl = null;
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "sponsor_funds"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var fund = _mapper.Map<SponsorFund>(dto);
            fund.ProofImageUrl = imageUrl;

            await _unitOfWork.SponsorFund.AddAsync(fund);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<SponsorFundReadDTO>(fund);
            return CreatedAtAction(nameof(GetById), new { id = fund.SponsorFundId }, ResultModel.Created(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile? file, [FromForm] SponsorFundUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var fund = await _unitOfWork.SponsorFund.GetByIdAsync(id);
            if (fund == null)
                return NotFound(ResultModel.NotFound());

            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "sponsor_funds"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                fund.ProofImageUrl = uploadResult.SecureUrl.ToString();
            }

            _mapper.Map(dto, fund);

            _unitOfWork.SponsorFund.Update(fund);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fund = await _unitOfWork.SponsorFund.GetByIdAsync(id);
            if (fund == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.SponsorFund.Delete(fund);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
