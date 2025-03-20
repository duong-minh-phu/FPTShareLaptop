using AutoMapper;
using BusinessObjects.Models;
using DataAccess.CompensationTransactionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompensationTransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompensationTransactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/CompensationTransaction
        [HttpGet]
        public async Task<IActionResult> GetCompensationTransactions()
        {
            var transactions = await _unitOfWork.CompensationTransaction.GetAllAsync();
            var transactionDTOs = _mapper.Map<IEnumerable<CompensationTransactionDTO>>(transactions);
            return Ok(transactionDTOs);
        }

        // GET: api/CompensationTransaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompensationTransaction(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            var transactionDTO = _mapper.Map<CompensationTransactionDTO>(transaction);
            return Ok(transactionDTO);
        }

        // POST: api/CompensationTransaction
        [HttpPost]
        public async Task<IActionResult> CreateCompensationTransaction([FromBody] CompensationTransactionDTO transactionDTO)
        {
            if (transactionDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var transaction = _mapper.Map<CompensationTransaction>(transactionDTO);
            await _unitOfWork.CompensationTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetCompensationTransaction), new { id = transaction.CompensationId }, _mapper.Map<CompensationTransactionDTO>(transaction));
        }

        // PUT: api/CompensationTransaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompensationTransaction(int id, [FromBody] CompensationTransactionDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.CompensationId != id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingTransaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            _mapper.Map(transactionDTO, existingTransaction);
            _unitOfWork.CompensationTransaction.Update(existingTransaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/CompensationTransaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompensationTransaction(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _unitOfWork.CompensationTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
