using AutoMapper;
using BusinessObjects.Models;
using DataAccess.BorrowHistoryDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/borrow-histories")]
    [ApiController]
    public class BorrowHistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BorrowHistoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/borrow-histories
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var borrowHistories = await _unitOfWork.BorrowHistory.GetAllAsync();
            var borrowHistoryDTOs = _mapper.Map<IEnumerable<BorrowHistoryReadDTO>>(borrowHistories);
            return Ok(ResultModel.Success(borrowHistoryDTOs));
        }

        // GET: api/borrow-histories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var borrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (borrowHistory == null)
            {
                return NotFound(ResultModel.NotFound("Borrow history not found."));
            }

            var borrowHistoryDTO = _mapper.Map<BorrowHistoryReadDTO>(borrowHistory);
            return Ok(ResultModel.Success(borrowHistoryDTO));
        }

        // POST: api/borrow-histories
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] BorrowHistoryCreateDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }

            var borrowHistory = _mapper.Map<BorrowHistory>(borrowHistoryDTO);
            await _unitOfWork.BorrowHistory.AddAsync(borrowHistory);            
            var donateItem = await _unitOfWork.DonateItem.GetByIdAsync(borrowHistoryDTO.ItemId);
            if (donateItem != null)
            {
                donateItem.TotalBorrowedCount += 1;
                _unitOfWork.DonateItem.Update(donateItem);
            }

            await _unitOfWork.SaveAsync();

            var createdDTO = _mapper.Map<BorrowHistoryReadDTO>(borrowHistory);
            return Ok(ResultModel.Created(createdDTO));
        }

        // PUT: api/borrow-histories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BorrowHistoryUpdateDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }

            var existingBorrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (existingBorrowHistory == null)
            {
                return NotFound(ResultModel.NotFound("Borrow history not found."));
            }

            _mapper.Map(borrowHistoryDTO, existingBorrowHistory);
            _unitOfWork.BorrowHistory.Update(existingBorrowHistory);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Borrow history updated successfully."));
        }

        // DELETE: api/borrow-histories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var borrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (borrowHistory == null)
            {
                return NotFound(ResultModel.NotFound("Borrow history not found."));
            }

            _unitOfWork.BorrowHistory.Delete(borrowHistory);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Borrow history deleted successfully."));
        }

        [HttpGet("by-sponsor/{sponsorUserId}")]
        public async Task<IActionResult> GetBorrowHistoriesBySponsorAsync(int sponsorUserId)
        {
            // 1. Lấy tất cả DonateItem của Sponsor
            var allDonateItems = await _unitOfWork.DonateItem.GetAllAsync();
            var donateItems = allDonateItems.Where(d => d.UserId == sponsorUserId).ToList();
            var donateItemIds = donateItems.Select(d => d.ItemId).ToList();

            if (!donateItemIds.Any())
            {
                return Ok(ResultModel.Success(new List<BorrowHistoryReadDTO>(), "No items donated by this sponsor."));
            }

            // 2. Lấy tất cả BorrowHistory liên quan các ItemId đó
            var allBorrowHistories = await _unitOfWork.BorrowHistory.GetAllAsync();
            var borrowHistories = allBorrowHistories.Where(bh => donateItemIds.Contains(bh.ItemId)).ToList();

            var borrowHistoryDTOs = _mapper.Map<IEnumerable<BorrowHistoryReadDTO>>(borrowHistories);

            return Ok(ResultModel.Success(borrowHistoryDTOs));
        }
    }

}
