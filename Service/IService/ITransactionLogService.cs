using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccess.TransactionLogDTO;

namespace Service.IService
{
    public interface ITransactionLogService
    {
        Task<List<TransactionLogResModel>> GetAllTransactionLogsAsync(string token);
    }

}
