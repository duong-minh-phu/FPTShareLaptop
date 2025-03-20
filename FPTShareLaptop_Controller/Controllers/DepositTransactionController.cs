using AutoMapper;
using BusinessObjects.Models;
using DataAccess.DepositTransactionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositTransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepositTransactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/DepositTransaction
        [HttpGet]
        public async Task<IActionResult> GetDepositTransactions()
        {
            var transactions = await _unitOfWork.DepositTransaction.GetAllAsync();
            var transactionDTOs = _mapper.Map<IEnumerable<DepositTransactionDTO>>(transactions);
            return Ok(transactionDTOs);
        }

        // GET: api/DepositTransaction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepositTransaction(int id)
        {
            var transaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            var transactionDTO = _mapper.Map<DepositTransactionDTO>(transaction);
            return Ok(transactionDTO);
        }

        // POST: api/DepositTransaction
        [HttpPost]
        public async Task<IActionResult> CreateDepositTransaction([FromBody] DepositTransactionDTO transactionDTO)
        {
            if (transactionDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var transaction = _mapper.Map<DepositTransaction>(transactionDTO);
            await _unitOfWork.DepositTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetDepositTransaction), new { id = transaction.DepositId }, _mapper.Map<DepositTransactionDTO>(transaction));
        }

        // PUT: api/DepositTransaction/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepositTransaction(int id, [FromBody] DepositTransactionDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.DepositId != id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingTransaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            _mapper.Map(transactionDTO, existingTransaction);
            _unitOfWork.DepositTransaction.Update(existingTransaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/DepositTransaction/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepositTransaction(int id)
        {
            var transaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _unitOfWork.DepositTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
