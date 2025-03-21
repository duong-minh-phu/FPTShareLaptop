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
            var borrowHistoryDTOs = _mapper.Map<IEnumerable<BorrowHistoryDTO>>(borrowHistories);
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

            var borrowHistoryDTO = _mapper.Map<BorrowHistoryDTO>(borrowHistory);
            return Ok(ResultModel.Success(borrowHistoryDTO));
        }

        // POST: api/borrow-histories
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] BorrowHistoryDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }

            var borrowHistory = _mapper.Map<BorrowHistory>(borrowHistoryDTO);
            await _unitOfWork.BorrowHistory.AddAsync(borrowHistory);
            await _unitOfWork.SaveAsync();

            var createdDTO = _mapper.Map<BorrowHistoryDTO>(borrowHistory);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = borrowHistory.BorrowHistoryId }, ResultModel.Created(createdDTO));
        }

        // PUT: api/borrow-histories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BorrowHistoryDTO borrowHistoryDTO)
        {
            if (borrowHistoryDTO == null || borrowHistoryDTO.BorrowHistoryId != id)
            {
                return BadRequest(ResultModel.BadRequest("ID mismatch."));
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
    }

}
