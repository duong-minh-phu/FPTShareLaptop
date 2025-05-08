using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.RefundTransactionDTO;
using DataAccess.TransactionLogDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;

        public TransactionLogService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jwtService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<List<TransactionLogResModel>> GetAllTransactionLogsAsync(string token)
        {
            var userId = int.Parse(_jwtService.decodeToken(token, "userId"));
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var transactions = await _unitOfWork.TransactionLog.GetAllAsync();

            // Nếu là Shop (RoleId = 6), chỉ hiển thị Withdraw và TransferIn
            if (user.RoleId == 6) // Shop
            {
                transactions = transactions
                    .Where(t =>
                        (t.TransactionType == "Withdraw" || t.TransactionType == "TransferIn") &&
                        t.UserId == userId) // Chỉ log của shop đó
                    .ToList();
            }


            return _mapper.Map<List<TransactionLogResModel>>(transactions);
        }

    }
}
