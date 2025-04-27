using AutoMapper;
using BusinessObjects.Models;
using DataAccess.CompensationTransactionDTO;
using DataAccess.ResultModel;
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
            var result = _mapper.Map<IEnumerable<CompensationTransactionDTO>>(transactions);

            return Ok(ResultModel.Success(result, "Lấy danh sách giao dịch bồi thường thành công"));
        }

        // GET: api/compensation-transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null)
                return NotFound(ResultModel.NotFound("Không tìm thấy giao dịch bồi thường"));

            var result = _mapper.Map<CompensationTransactionDTO>(transaction);
            return Ok(ResultModel.Success(result, "Lấy chi tiết giao dịch bồi thường thành công"));
        }

        // POST: api/compensation-transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompensationTransactionCreateDTO transactionDTO)
        {
            if (transactionDTO == null)
                return BadRequest(ResultModel.BadRequest("Dữ liệu không hợp lệ"));

            var transaction = _mapper.Map<CompensationTransaction>(transactionDTO);
            await _unitOfWork.CompensationTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            var deposit = await _unitOfWork.DepositTransaction.GetByIdAsync(transaction.DepositTransactionId);
            decimal usedDepositAmount = 0;
            decimal extraPaymentRequired = 0;
            decimal refundAmount = 0;

            // Xác định số tiền bồi thường
            if (transaction.CompensationAmount == 0)
            {
                // Không có thiệt hại -> hoàn toàn bộ tiền cọc
                refundAmount = deposit.Amount;
            }
            else if (transaction.CompensationAmount < deposit.Amount)
            {
                // Thiệt hại nhỏ hơn tiền cọc -> trừ vào tiền cọc, trả lại phần dư
                usedDepositAmount = transaction.CompensationAmount;
                refundAmount = deposit.Amount - usedDepositAmount;
            }
            else if (transaction.CompensationAmount == deposit.Amount)
            {
                // Thiệt hại đúng bằng tiền cọc -> dùng hết tiền cọc, không hoàn lại
                usedDepositAmount = deposit.Amount;
                refundAmount = 0;
            }
            else
            {
                // Thiệt hại lớn hơn tiền cọc -> lấy hết tiền cọc + khách phải trả thêm
                usedDepositAmount = deposit.Amount;
                extraPaymentRequired = transaction.CompensationAmount - deposit.Amount;
                refundAmount = 0;
            }

            // Lưu log giao dịch
            var log = new TransactionLog
            {
                UserId = transaction.UserId,
                TransactionType = "Compensation",
                Amount = transaction.CompensationAmount,
                ExtraPaymentRequired = extraPaymentRequired,
                UsedDepositAmount = usedDepositAmount,
                CreatedDate = DateTime.UtcNow,
                Note = $"Compensation transaction created for contract {transaction.ContractId}, " +
                       $"compensation amount: {transaction.CompensationAmount}, " +
                       $"used deposit: {usedDepositAmount}, extra payment required: {extraPaymentRequired}",
                ReferenceId = transaction.CompensationId,
                SourceTable = "CompensationTransaction"
            };

            await _unitOfWork.TransactionLog.AddAsync(log);
            await _unitOfWork.SaveAsync();



            var result = _mapper.Map<CompensationTransactionDTO>(transaction);
            return Ok(ResultModel.Created(result, "Tạo giao dịch bồi thường thành công"));

        }

        // PUT: api/compensation-transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompensationTransactionUpdateDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.CompensationId != id)
                return BadRequest(ResultModel.BadRequest("ID không khớp hoặc dữ liệu không hợp lệ"));

            var existing = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (existing == null)
                return NotFound(ResultModel.NotFound("Không tìm thấy giao dịch để cập nhật"));

            _mapper.Map(transactionDTO, existing);
            _unitOfWork.CompensationTransaction.Update(existing);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Cập nhật giao dịch bồi thường thành công"));
        }

        // DELETE: api/compensation-transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _unitOfWork.CompensationTransaction.GetByIdAsync(id);
            if (transaction == null)
                return NotFound(ResultModel.NotFound("Không tìm thấy giao dịch để xóa"));

            _unitOfWork.CompensationTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Xóa giao dịch bồi thường thành công"));
        }
    }
}
