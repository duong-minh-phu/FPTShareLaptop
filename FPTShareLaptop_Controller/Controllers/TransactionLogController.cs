using DataAccess.ResultModel;
using DataAccess.TransactionLogDTO;
using Microsoft.AspNetCore.Mvc;
using Service.IService;  
using Service.Service;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionLogController : ControllerBase
    {
        private readonly ITransactionLogService _transactionLogService;
        
        public TransactionLogController(ITransactionLogService transactionLogService)
        {
            _transactionLogService = transactionLogService;
        }

        // GET: api/TransactionLog
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllTransactionLogs()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _transactionLogService.GetAllTransactionLogsAsync(token);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = result,
                Message = "Get transaction log successfully"
            };
            return StatusCode(response.Code, response);
        }

    }
}
