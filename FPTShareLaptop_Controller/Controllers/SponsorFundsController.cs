using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ResultModel;
using DataAccess.SponsorFundDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/sponsor-funds")]
    [ApiController]
    public class SponsorFundsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly Sep490Context _context;
        public SponsorFundsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary, Sep490Context context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _context = context;
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
            fund.TransferDate = DateTime.UtcNow;

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


        [HttpGet("sponsor-summary/{userId}")]
        public async Task<IActionResult> GetSponsorSummary(int userId)
        {
            // 1. Lấy toàn bộ các SponsorFund của user đó
            var sponsorFunds = await _context.SponsorFunds
    .Where(sf => sf.UserId == userId)
    .Include(sf => sf.SponsorContributions)
    .ToListAsync();


            if (!sponsorFunds.Any())
            {
                return NotFound(ResultModel.NotFound("Không tìm thấy dữ liệu tài trợ cho người dùng này."));
            }

            // 2. Tổng số tiền đã donate
            var totalDonated = sponsorFunds.Sum(sf => sf.Amount);

            // 3. Tổng số tiền đã dùng để mua laptop
            var totalUsed = sponsorFunds
                .SelectMany(sf => sf.SponsorContributions)
                .Sum(sc => sc.ContributedAmount);

            // 4. Số dư còn lại
            var remainingAmount = totalDonated - totalUsed;

            var result = new
            {
                TotalDonated = totalDonated,
                TotalUsed = totalUsed,
                RemainingAmount = remainingAmount
            };

            return Ok(ResultModel.Success(result));
        }

        [HttpGet("summary/{userId}")]
        public async Task<IActionResult> GetSponsorSummaryadmin(int userId)
        {
            var sponsorFunds = await _context.SponsorFunds
                .Where(sf => sf.UserId == userId)
                .Include(sf => sf.SponsorContributions)
                .ToListAsync();

            if (!sponsorFunds.Any())
            {
                return NotFound(new
                {
                    Message = $"Không tìm thấy khoản tài trợ nào cho UserId = {userId}"
                });
            }

            var totalDonated = sponsorFunds.Sum(sf => sf.Amount);
            var totalContributed = sponsorFunds
                .SelectMany(sf => sf.SponsorContributions)
                .Sum(sc => sc.ContributedAmount);
            var remaining = totalDonated - totalContributed;

            var result = new
            {
                UserId = userId,
                TotalDonated = totalDonated,
                TotalContributed = totalContributed,
                RemainingAmount = remaining,
                FundDetails = sponsorFunds.Select(f => new
                {
                    f.SponsorFundId,
                    f.Amount,
                    f.CreatedDate,
                    Contributions = f.SponsorContributions.Select(c => new
                    {
                        c.SponsorContributionId,
                        c.PurchasedLaptopId,
                        c.ContributedAmount
                    })
                })
            };

            return Ok(result);
        }


    }
}
