using AutoMapper;
using BusinessObjects.Models;
using DataAccess.BorrowHistoryDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
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

        // GET: api/BorrowHistory
        [HttpGet]
        public async Task<IActionResult> GetBorrowHistories()
        {
            var borrowHistories = await _unitOfWork.BorrowHistory.GetAllAsync();
            var borrowHistoryDTOs = _mapper.Map<IEnumerable<BorrowHistoryDTO>>(borrowHistories);
            return Ok(borrowHistoryDTOs);
        }

        // GET: api/BorrowHistory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowHistory(int id)
        {
            var borrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (borrowHistory == null)
            {
                return NotFound();
            }
            var borrowHistoryDTO = _mapper.Map<BorrowHistoryDTO>(borrowHistory);
            return Ok(borrowHistoryDTO);
        }

        // POST: api/BorrowHistory
        [HttpPost]
        public async Task<IActionResult> CreateBorrowHistory([FromBody] BorrowHistoryDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var borrowHistory = _mapper.Map<BorrowHistory>(borrowHistoryDTO);
            await _unitOfWork.BorrowHistory.AddAsync(borrowHistory);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetBorrowHistory), new { id = borrowHistory.BorrowHistoryId }, _mapper.Map<BorrowHistoryDTO>(borrowHistory));
        }

        // PUT: api/BorrowHistory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowHistory(int id, [FromBody] BorrowHistoryDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null || borrowHistoryDTO.BorrowHistoryId != id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingBorrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (existingBorrowHistory == null)
            {
                return NotFound();
            }

            _mapper.Map(borrowHistoryDTO, existingBorrowHistory);
            _unitOfWork.BorrowHistory.Update(existingBorrowHistory);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/BorrowHistory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowHistory(int id)
        {
            var borrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(id);
            if (borrowHistory == null)
            {
                return NotFound();
            }

            _unitOfWork.BorrowHistory.Delete(borrowHistory);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
