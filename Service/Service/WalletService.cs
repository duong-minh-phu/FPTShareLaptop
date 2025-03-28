using System.Net;
using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.WalletDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IJWTService jwtService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        // Lấy ví theo UserId từ JWT
        public async Task<WalletResModel> GetWalletById(int walletId)
        {          
            var wallet = await _unitOfWork.Wallet.GetByIdAsync(walletId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            return _mapper.Map<WalletResModel>(wallet);
        }

        // Tạo ví mới
        public async Task<WalletResModel> CreateWallet(string token, WalletReqModel model)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var existingWallet = await _unitOfWork.Wallet.GetByIdAsync(userId);
            if (existingWallet != null)
                throw new ApiException(HttpStatusCode.BadRequest, "User already has a wallet.");

            var wallet = _mapper.Map<Wallet>(model);
            wallet.UserId = user.UserId; 
            wallet.Status = WalletEnum.Active.ToString(); 
            wallet.CreatedDate = DateTime.UtcNow; 

            await _unitOfWork.Wallet.AddAsync(wallet);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<WalletResModel>(wallet);
        }

        // Nạp tiền vào ví
        public async Task Deposit(string token, decimal amount)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var wallet = await _unitOfWork.Wallet.GetByIdAsync(userId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            wallet.Balance += amount;

            _unitOfWork.Wallet.Update(wallet);
            await _unitOfWork.SaveAsync();
        }

        // Rút tiền khỏi ví
        public async Task Withdraw(string token, decimal amount)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var wallet = await _unitOfWork.Wallet.GetByIdAsync(userId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            if (wallet.Balance < amount)
                throw new ApiException(HttpStatusCode.BadRequest, "Insufficient balance.");

            wallet.Balance -= amount;

            _unitOfWork.Wallet.Update(wallet);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<WalletResModel>> GetAllWallets()
        {
            var wallets = await _unitOfWork.Wallet.GetAllAsync();
            return _mapper.Map<List<WalletResModel>>(wallets);
        }
    }
}
