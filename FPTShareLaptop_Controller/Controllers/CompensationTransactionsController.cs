using AutoMapper;
using BusinessObjects.Models;
using DataAccess.CompensationTransactionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/compensation-transactions")]
    [ApiController]
    public class CompensationTransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompensationTransactionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/compensation-transactions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _unitOfWork.CompensationTransaction.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CompensationTransactionDTO>>(transactions));
        }

        // GET: api/compensation-transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(_mapper.Map<CompensationTransactionDTO>(transaction));
        }

        // POST: api/compensation-transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompensationTransactionCreateDTO transactionDTO)
        {
            if (transactionDTO == null) return BadRequest("Invalid data.");

            var transaction = _mapper.Map<CompensationTransaction>(transactionDTO);
            await _unitOfWork.CompensationTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            var log = new TransactionLog
            {
                UserId = transaction.UserId,
                TransactionType = "Compensation",
                Amount = transaction.CompensationAmount,
                ExtraPaymentRequired = transactionDTO.ExtraPaymentRequired,
                UsedDepositAmount = transactionDTO.UsedDepositAmount,
                CreatedDate = DateTime.UtcNow,
                Note = $"Compensation transaction created for contract {transaction.ContractId}, amount: {transaction.CompensationAmount}, used deposit: {transaction.UsedDepositAmount}, extra payment required: {transaction.ExtraPaymentRequired}",
                ReferenceId = transaction.CompensationId,
                SourceTable = "CompensationTransaction"
            };

            await _unitOfWork.TransactionLog.AddAsync(log);
            await _unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = transaction.CompensationId },
                _mapper.Map<CompensationTransactionDTO>(transaction));
        }

        // PUT: api/compensation-transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompensationTransactionUpdateDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.CompensationId != id)
                return BadRequest("ID mismatch.");

            var existingTransaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (existingTransaction == null) return NotFound();

            _mapper.Map(transactionDTO, existingTransaction);
            _unitOfWork.CompensationTransaction.Update(existingTransaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/compensation-transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            _unitOfWork.CompensationTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
