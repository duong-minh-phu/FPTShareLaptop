using System.Net;
using BusinessObjects.Models;
using Service.IService;
using BusinessObjects.Enums;
using DataAccess.BorrowContractDTO;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class BorrowContractService : IBorrowContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;

        public BorrowContractService(IUnitOfWork unitOfWork, IJWTService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        // Lấy tất cả hợp đồng mượn
        public async Task<List<BorrowContractResponseDTO>> GetAllBorrowContracts()
        {
            var contracts = await _unitOfWork.BorrowContract.GetAllAsync();
            return contracts.Select(contract => new BorrowContractResponseDTO
            {
                ContractId = contract.ContractId,
                RequestId = contract.RequestId,
                ItemId = contract.ItemId,
                UserId = contract.UserId,
                FullName = contract.User.FullName,  
                Email = contract.User.Email,
                PhoneNumber = contract.User.PhoneNumber,
                Status = contract.Status,
                ContractDate = contract.ContractDate,
                Terms = contract.Terms,
                ConditionBorrow = contract.ConditionBorrow,
                ItemValue = contract.ItemValue,
                ExpectedReturnDate = contract.ExpectedReturnDate
            }).ToList();
        }


        // Lấy hợp đồng theo ID
        public async Task<BorrowContractResponseDTO> GetBorrowContractById(int contractId)
        {
            var contract = await _unitOfWork.BorrowContract.GetByIdAsync(contractId);
            if (contract == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow contract not found.");

            return new BorrowContractResponseDTO
            {
                ContractId = contract.ContractId,
                RequestId = contract.RequestId,
                ItemId = contract.ItemId,
                UserId = contract.UserId,
                FullName = contract.User.FullName,
                Email = contract.User.Email,
                PhoneNumber = contract.User.PhoneNumber,
                Status = contract.Status,
                ContractDate = contract.ContractDate,
                Terms = contract.Terms,
                ConditionBorrow = contract.ConditionBorrow,
                ItemValue = contract.ItemValue,
                ExpectedReturnDate = contract.ExpectedReturnDate
            };
        }


        // Tạo hợp đồng mới
        public async Task CreateBorrowContract(string token, CreateBorrowContractReqModel request)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var borrowRequest = await _unitOfWork.BorrowRequest.GetByIdAsync(request.RequestId);
            if (borrowRequest == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow request not found.");

            var existingContract = await _unitOfWork.BorrowContract.FirstOrDefaultAsync(br =>
            br.UserId == int.Parse(userId) &&
            br.ItemId == request.ItemId &&
            (br.Status == DonateStatus.Pending.ToString()));
            if (existingContract != null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Bạn đã có hợp đồng mượn cho laptop này.");
            }

            var contract = new BorrowContract
            {
                RequestId = request.RequestId,
                ItemId = request.ItemId,
                UserId = user.UserId,
                Status = ContractStatus.Pending.ToString(),
                ContractDate = DateTime.UtcNow,
                Terms = request.Terms,
                ConditionBorrow = request.ConditionBorrow,
                ItemValue = request.ItemValue,
                ExpectedReturnDate = request.ExpectedReturnDate
            };

            await _unitOfWork.BorrowContract.AddAsync(contract);
            await _unitOfWork.SaveAsync();
        }

        // Cập nhật hợp đồng
        public async Task UpdateBorrowContract(string token, int contractId, UpdateBorrowContractReqModel updateModel)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var contract = await _unitOfWork.BorrowContract.GetByIdAsync(contractId);
            if (contract == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow contract not found.");

            contract.Status = updateModel.Status ?? contract.Status;
            contract.Terms = updateModel.Terms ?? contract.Terms;
            contract.ConditionBorrow = updateModel.ConditionBorrow ?? contract.ConditionBorrow;
            contract.ItemValue = updateModel.ItemValue ?? contract.ItemValue;
            contract.ExpectedReturnDate = updateModel.ExpectedReturnDate ?? contract.ExpectedReturnDate;

            _unitOfWork.BorrowContract.Update(contract);
            await _unitOfWork.SaveAsync();
        }

        // Xóa hợp đồng (soft delete)
        public async Task DeleteBorrowContract(string token, int contractId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var contract = await _unitOfWork.BorrowContract.GetByIdAsync(contractId);
            if (contract == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow contract not found.");

            _unitOfWork.BorrowContract.Delete(contract);
            await _unitOfWork.SaveAsync();
        }
    }
}
