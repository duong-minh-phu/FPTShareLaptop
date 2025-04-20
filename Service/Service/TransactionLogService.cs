using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.RefundTransactionDTO;
using DataAccess.TransactionLogDTO;
using Service.IService;

namespace Service.Service
{
    public class TransactionLogService : ITransactionLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TransactionLogResModel>> GetAllTransactionLogsAsync()
        {
            var transactions = await _unitOfWork.TransactionLog.GetAllAsync();
            return _mapper.Map<List<TransactionLogResModel>>(transactions);
        }
    }
}
