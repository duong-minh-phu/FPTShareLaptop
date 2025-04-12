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
        Task<List<WalletTransactionResModel>> GetAllTransactions();
        Task<List<WalletTransactionResModel>> GetTransactionsByUser(string token);
        Task<WalletTransactionResModel> GetTransactionById(int transactionId);
        Task DeleteTransaction(int transactionId);
    }
}
