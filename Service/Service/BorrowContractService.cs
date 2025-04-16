using System.Net;
using BusinessObjects.Models;
using Service.IService;
using BusinessObjects.Enums;
using DataAccess.BorrowContractDTO;
using Service.Utils.CustomException;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using DataAccess.BorrowRequestDTO;
using System.Linq.Expressions;

namespace Service.Service
{
    public class BorrowContractService : IBorrowContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly Cloudinary _cloudinary;

        public BorrowContractService(IUnitOfWork unitOfWork, IJWTService jwtService, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _cloudinary = cloudinary;
        }

        // Lấy tất cả hợp đồng mượn
        public async Task<List<BorrowContractResponseModel>> GetAllBorrowContracts()
        {
            var contracts = await _unitOfWork.BorrowContract.GetAllAsync(
                includeProperties: new Expression<Func<BorrowContract, object>>[] {
                    c => c.User,
                    c => c.ContractImages
                });

            return contracts.Select(contract => new BorrowContractResponseModel
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
                ExpectedReturnDate = contract.ExpectedReturnDate,
                ContractImages = contract.ContractImages.Select(ci => ci.ImageUrl).ToList()
            }).ToList();
        }


        // Lấy hợp đồng theo ID
        public async Task<BorrowContractResponseModel> GetBorrowContractById(int contractId)
        {
            var contract = await _unitOfWork.BorrowContract.GetByIdAsync(contractId,
               includeProperties: new Expression<Func<BorrowContract, object>>[] {
                    c => c.User,
                    c => c.ContractImages
               });

            if (contract == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow contract not found.");

            return new BorrowContractResponseModel
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
                ExpectedReturnDate = contract.ExpectedReturnDate,
                ContractImages = contract.ContractImages.Select(ci => ci.ImageUrl).ToList()
            };
        }


        // Tạo hợp đồng mới
        public async Task<BorrowContractResponseModel> CreateBorrowContract(string token, CreateBorrowContractReqModel request)
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
            var item = await _unitOfWork.DonateItem.GetByIdAsync(request.ItemId);
            if (item == null)
                throw new ApiException(HttpStatusCode.NotFound, "Laptop not found.");

            item.Status = "Borrwing";
            _unitOfWork.DonateItem.Update(item);
            await _unitOfWork.SaveAsync();

            // Trả về BorrowContractResponseDTO
            return new BorrowContractResponseModel
            {
                ContractId = contract.ContractId,
                RequestId = contract.RequestId,
                ItemId = contract.ItemId,
                UserId = contract.UserId,
                FullName = user.FullName,  
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Status = contract.Status,
                ContractDate = contract.ContractDate,
                Terms = contract.Terms,
                ConditionBorrow = contract.ConditionBorrow,
                ItemValue = contract.ItemValue,
                ExpectedReturnDate = contract.ExpectedReturnDate
            };
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

        public async Task UploadSignedContractImage(string token, int contractId, UploadBorrowContractReqModel requestModel)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var contract = await _unitOfWork.BorrowContract.GetByIdAsync(contractId, includeProperties: c => c.ContractImages);
            if (contract == null)
                throw new ApiException(HttpStatusCode.NotFound, "Borrow contract not found.");          

            // Upload ảnh
            using var stream = requestModel.ImageBorrowConract.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(requestModel.ImageBorrowConract.FileName, stream),
                Folder = "contract_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
                throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);

            // Thêm ảnh vào danh sách
            contract.ContractImages.Add(new ContractImage
            {
                ImageUrl = uploadResult.SecureUrl.ToString(),
                BorrowContractId = contract.ContractId,
                CreatedDate = DateTime.UtcNow
            });

            // Cập nhật trạng thái
            contract.Status = ContractStatus.Signed.ToString();

            _unitOfWork.BorrowContract.Update(contract);
            await _unitOfWork.SaveAsync();
        }

    }
}
