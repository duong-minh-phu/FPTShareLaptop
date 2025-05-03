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
        Task DisburseToManagerAsync(decimal amount);
        Task TransferFromManagerToShopsAsync(string token, List<ShopTransferReqModel> transfers, decimal feeRate);
        Task WithdrawFromManagerAsync(decimal amount);
    }
}
