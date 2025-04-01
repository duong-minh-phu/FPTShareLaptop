using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.WalletDTO;

namespace Service.IService
{
    public interface IWalletService
    {
        Task<List<WalletResModel>> GetAllWallets();
        Task<WalletResModel> GetWalletById(int walletId);
        Task<WalletResModel> CreateWallet(string token, WalletReqModel model);
        Task Deposit(string token, decimal amount);
        Task Withdraw(string token, decimal amount);
    }
}
