using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccess.RefundTransactionDTO;

namespace Service.IService
{
    public interface IRefundTransactionService
    {
        Task<List<RefundTransactionResModel>> GetAllAsync();
        Task<RefundTransactionResModel> GetByIdAsync(int id);
        Task<List<RefundTransactionResModel>> GetByWalletIdAsync(int walletId);
        Task AddAsync(RefundTransactionReqModel refund);
        Task UpdateAsync(string token, RefundTransactionReqModel refund, int refundId);
        Task DeleteAsync(string token, int refundId);
    }
}
