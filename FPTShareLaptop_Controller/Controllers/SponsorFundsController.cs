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
        public async Task<IActionResult> GetSponsorSummaryAdmin(int userId)
        {
            var sponsorFunds = await _context.SponsorFunds
                .Where(sf => sf.UserId == userId)
                .Include(sf => sf.SponsorContributions)
                    .ThenInclude(sc => sc.PurchasedLaptop)  // Bao gồm PurchasedLaptop
                        .ThenInclude(pl => pl.Item)        // Bao gồm DonateItem (Item)
                .ToListAsync();

            if (!sponsorFunds.Any())
            {
                return NotFound(ResultModel.NotFound($"Không tìm thấy khoản tài trợ nào cho UserId = {userId}"));
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
                        c.ContributedAmount,
                        DonateItem = new
                        {
                            // Thêm thông tin từ DonateItem liên kết với PurchasedLaptop
                            c.PurchasedLaptop.Item.ItemId,
                            c.PurchasedLaptop.Item.ItemName,
                            c.PurchasedLaptop.Item.ItemImage,
                            c.PurchasedLaptop.Item.SerialNumber,
                            c.PurchasedLaptop.Item.Model,
                            c.PurchasedLaptop.Item.Color,
                            c.PurchasedLaptop.Item.ConditionItem,
                            c.PurchasedLaptop.Item.TotalBorrowedCount,
                            c.PurchasedLaptop.Item.Status
                        }
                    })
                })
            };

            return Ok(ResultModel.Success(result, "Lấy thông tin tài trợ thành công"));
        }


        [HttpGet("fund-summary")]
        public async Task<IActionResult> GetFundSummary()
        {
            // Lấy tổng số tiền nhận được từ các khoản tài trợ
            var sponsorFunds = await _context.SponsorFunds.ToListAsync();
            var totalReceived = sponsorFunds.Sum(sf => sf.Amount); // Tổng số tiền nhận được từ các khoản tài trợ

            // Lấy tổng số tiền đã chi tiêu từ các laptop đã mua
            var purchasedLaptops = await _context.PurchasedLaptops.ToListAsync();
            var totalUsed = purchasedLaptops.Sum(pl => pl.TotalPrice); // Tổng số tiền đã chi tiêu từ việc mua laptop

            // Tính số tiền còn lại
            var remainingAmount = totalReceived - totalUsed;

            // Trả về kết quả
            var result = new
            {
                TotalReceived = totalReceived,
                TotalUsed = totalUsed,
                RemainingAmount = remainingAmount
            };

            return Ok(ResultModel.Success(result, "Tổng kết quỹ tài trợ"));
        }


        [HttpGet("users-with-remaining-funds")]
        public async Task<IActionResult> GetUsersWithRemainingFunds()
        {
            // Lấy tất cả các khoản tài trợ và đóng góp cho laptop đã mua
            var sponsorFunds = await _context.SponsorFunds.Include(sf => sf.User).ToListAsync();
            var sponsorContributions = await _context.SponsorContributions.Include(sc => sc.PurchasedLaptop).ToListAsync();

            // Tính toán số tiền nhận được và đã chi tiêu cho từng user
            var userSummary = sponsorFunds
                .GroupBy(sf => sf.UserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    TotalReceived = group.Sum(sf => sf.Amount), // Tổng số tiền nhận được từ SponsorFund
                    TotalUsed = sponsorContributions
                        .Where(sc => sc.SponsorFundId == group.Key) // Lọc các khoản đóng góp của user này
                        .Sum(sc => sc.ContributedAmount), // Tổng số tiền đã chi tiêu
                    RemainingAmount = group.Sum(sf => sf.Amount) - sponsorContributions
                        .Where(sc => sc.SponsorFundId == group.Key)
                        .Sum(sc => sc.ContributedAmount) // Số tiền còn lại
                })
                .Where(user => user.RemainingAmount > 0) // Chỉ lấy những người dùng còn tiền
                .ToList();

            if (!userSummary.Any())
            {
                return NotFound(new { Message = "Không có người dùng nào có tiền còn lại." });
            }

            // Trả về kết quả
            return Ok(ResultModel.Success(userSummary, "Danh sách người dùng còn dư tiền"));
        }

        [HttpGet("buyers-of-laptop/{laptopId}")]
        public async Task<IActionResult> GetBuyersOfLaptop(int laptopId)
        {
            // Lấy tất cả các đóng góp liên quan đến laptop có ID bằng laptopId
            var contributions = await _context.SponsorContributions
                .Where(sc => sc.PurchasedLaptopId == laptopId)
                .Include(sc => sc.SponsorFund) // Lấy thông tin quỹ tài trợ liên quan đến từng đóng góp
                .ThenInclude(sf => sf.User) // Lấy thông tin người dùng từ SponsorFund
                .ToListAsync();

            // Kiểm tra nếu không có ai đã đóng góp cho laptop này
            if (!contributions.Any())
            {
                return NotFound(new { Message = $"Không có người dùng nào đóng góp cho laptop có ID = {laptopId}" });
            }

            // Lấy thông tin người dùng đã đóng góp
            var buyers = contributions.Select(sc => new
            {
                UserId = sc.SponsorFund.UserId,
                UserName = sc.SponsorFund.User.FullName, // Giả sử bạn có thuộc tính Name trong User
                ContributedAmount = sc.ContributedAmount,
                PurchasedLaptopId = sc.PurchasedLaptopId
            }).ToList();

            // Trả về kết quả
            return Ok(ResultModel.Success(buyers, "Danh sách người dùng đã đóng góp cho laptop"));
        }

        [HttpGet("donate-items-by-user/{userId}")]
        public async Task<IActionResult> GetDonateItemsByUserId(int userId)
        {
            // Lấy tất cả các quỹ tài trợ của userId
            var sponsorFunds = await _context.SponsorFunds
                .Where(sf => sf.UserId == userId)
                .Include(sf => sf.SponsorContributions)
                .ThenInclude(sc => sc.PurchasedLaptop) // Bao gồm PurchasedLaptop để tìm laptop đã mua
                .ToListAsync();

            if (!sponsorFunds.Any())
            {
                return NotFound(new { Message = $"Không tìm thấy quỹ tài trợ nào cho UserId = {userId}" });
            }

            // Lấy tất cả các PurchasedLaptops từ SponsorContributions
            var purchasedLaptops = sponsorFunds
                .SelectMany(sf => sf.SponsorContributions)
                .Select(sc => sc.PurchasedLaptop)
                .Distinct()
                .ToList();

            // Tìm tất cả các DonateItem liên quan đến các PurchasedLaptop này
            var donatedItems = _context.DonateItems
                .Where(di => di.PurchasedLaptops.Any(pl => purchasedLaptops.Contains(pl)))
                .ToList();

            if (!donatedItems.Any())
            {
                return NotFound(new { Message = $"Không tìm thấy DonateItem nào được mua bằng tiền của UserId = {userId}" });
            }

            return Ok(ResultModel.Success(donatedItems, "Danh sách DonateItems đã được mua bằng tiền của User"));
        }



    }
}
