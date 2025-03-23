using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.WalletTransaction;

namespace Service.IService
{
    public interface IWalletTransactionService
    {
        Task<List<WalletTransactionResModel>> GetTransactionsByUser(string token);
        Task<WalletTransactionResModel> GetTransactionById(int transactionId);
        Task<WalletTransactionResModel> CreateTransaction(string token, WalletTransactionReqModel model);
        Task DeleteTransaction(string token, int transactionId);
    }
}
