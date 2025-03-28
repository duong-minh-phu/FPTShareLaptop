using AutoMapper;
using BusinessObjects.Models;
using DataAccess.ResultModel;
using DataAccess.SettlementTransactionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/settlement-transactions")]
    [ApiController]
    public class SettlementTransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SettlementTransactionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/settlement-transactions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _unitOfWork.SettlementTransaction.GetAllAsync();
            var transactionDTOs = _mapper.Map<IEnumerable<SettlementTransactionDTO>>(transactions);
            return Ok(ResultModel.Success(transactionDTOs));
        }

        // GET: api/settlement-transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _unitOfWork.SettlementTransaction.GetByIdAsync(id);
            if (transaction == null)
                return NotFound(ResultModel.NotFound());

            var transactionDTO = _mapper.Map<SettlementTransactionDTO>(transaction);
            return Ok(ResultModel.Success(transactionDTO));
        }

        // GET: api/settlement-transactions/by-order/{orderId}
        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var transactions = await _unitOfWork.Order.GetByIdAsync(orderId);
            var transactionDTOs = _mapper.Map<IEnumerable<SettlementTransactionDTO>>(transactions);
            return Ok(ResultModel.Success(transactionDTOs));
        }

        // POST: api/settlement-transactions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SettlementTransactionCreateDTO transactionDTO)
        {
            if (transactionDTO == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var transaction = _mapper.Map<SettlementTransaction>(transactionDTO);
            transaction.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.SettlementTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<SettlementTransactionDTO>(transaction);
            return CreatedAtAction(nameof(GetById), new { id = transaction.SettlementId }, ResultModel.Created(result));
        }

        // PUT: api/settlement-transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SettlementTransactionUpdateDTO transactionDTO)
        {
            if (transactionDTO == null || transactionDTO.Id != id)
                return BadRequest(ResultModel.BadRequest("ID mismatch."));

            var existingTransaction = await _unitOfWork.SettlementTransaction.GetByIdAsync(id);
            if (existingTransaction == null)
                return NotFound(ResultModel.NotFound());

            _mapper.Map(transactionDTO, existingTransaction);
            

            _unitOfWork.SettlementTransaction.Update(existingTransaction);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        // DELETE: api/settlement-transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _unitOfWork.SettlementTransaction.GetByIdAsync(id);
            if (transaction == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.SettlementTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
