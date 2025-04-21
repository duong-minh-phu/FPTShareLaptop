using AutoMapper;
using BusinessObjects.Models;
using DataAccess.DepositTransactionDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/deposit-transactions")]
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

        // GET: api/deposit-transactions
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var transactions = await _unitOfWork.DepositTransaction.GetAllAsync();
            var transactionDTOs = _mapper.Map<IEnumerable<DepositTransactionDTO>>(transactions);
            return Ok(ResultModel.Success(transactionDTOs));
        }

        // GET: api/deposit-transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var transaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound(ResultModel.NotFound("Deposit transaction not found."));
            }

            var transactionDTO = _mapper.Map<DepositTransactionDTO>(transaction);
            return Ok(ResultModel.Success(transactionDTO));
        }

        // POST: api/deposit-transactions
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] DepositTransactionCreateDTO transactionDTO)
        {
            if (transactionDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }

            var transaction = _mapper.Map<DepositTransaction>(transactionDTO);
            await _unitOfWork.DepositTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            var log = new TransactionLog
            {
                UserId = transaction.UserId, // Ghi nhận UserId từ token
                TransactionType = "Deposit", // Loại giao dịch là Deposit
                Amount = transaction.Amount, // Số tiền giao dịch
                CreatedDate = DateTime.UtcNow, // Thời gian tạo log
                Note = $"Deposit transaction created for contract {transaction.ContractId}, amount: {transaction.Amount}", // Ghi chú
                ReferenceId = transaction.DepositId, // Sử dụng DepositId làm ReferenceId
                SourceTable = "DepositTransaction" // Ghi lại bảng nguồn
            };

            await _unitOfWork.TransactionLog.AddAsync(log);
            await _unitOfWork.SaveAsync();
            var createdDTO = _mapper.Map<DepositTransactionDTO>(transaction);
            return Ok(ResultModel.Created(createdDTO));
        }

        // PUT: api/deposit-transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] DepositTransactionUpdateDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.DepositId != id)
            {
                return BadRequest(ResultModel.BadRequest("ID mismatch."));
            }

            var existingTransaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                return NotFound(ResultModel.NotFound("Deposit transaction not found."));
            }

            _mapper.Map(transactionDTO, existingTransaction);
            _unitOfWork.DepositTransaction.Update(existingTransaction);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deposit transaction updated successfully."));
        }

        // DELETE: api/deposit-transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var transaction = await _unitOfWork.DepositTransaction.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound(ResultModel.NotFound("Deposit transaction not found."));
            }

            _unitOfWork.DepositTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deposit transaction deleted successfully."));
        }
    }
}
