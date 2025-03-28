using System.Net;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.BorrowRequestDTO;
using Service.IService;
using Service.Service;
using Service.Utils.CustomException;

public class BorrowRequestService : IBorrowRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;

    public BorrowRequestService(IUnitOfWork unitOfWork, IJWTService jWTService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jWTService;
    }

    // Lấy tất cả yêu cầu mượn
    public async Task<List<BorrowRequestResModel>> GetAllBorrowRequests()
    {
        var borrowRequests = await _unitOfWork.BorrowRequest.GetAllAsync(includeProperties: b => b.Item);
        return borrowRequests.Select(b => new BorrowRequestResModel
        {
            RequestId = b.RequestId,
            UserId = b.UserId,
            ItemId = b.ItemId,
            ItemName = b.Item.ItemName,
            Status = b.Status,
            StartDate = b.StartDate,
            EndDate = b.EndDate
        }).ToList();
    }

    // Lấy yêu cầu mượn theo ID
    public async Task<BorrowRequestResModel> GetBorrowRequestById(int requestId)
    {
        var borrowRequest = await _unitOfWork.BorrowRequest.GetByIdAsync(requestId, includeProperties: b => b.Item);
        if (borrowRequest == null)
            throw new ApiException(HttpStatusCode.NotFound, "Yêu cầu mượn không tồn tại.");

        return new BorrowRequestResModel
        {
            RequestId = borrowRequest.RequestId,
            UserId = borrowRequest.UserId,
            ItemId = borrowRequest.ItemId,
            ItemName = borrowRequest.Item.ItemName,
            Status = borrowRequest.Status,
            StartDate = borrowRequest.StartDate,
            EndDate = borrowRequest.EndDate
        };
    }

    // Tạo yêu cầu mượn mới
    public async Task CreateBorrowRequest(string token, CreateBorrowRequestReqModel requestModel)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");

        var item = await _unitOfWork.DonateItem.GetByIdAsync(requestModel.ItemId);
        if (item == null)
            throw new ApiException(HttpStatusCode.NotFound, "Laptop not found.");
        
        var existingRequest = await _unitOfWork.BorrowRequest.FirstOrDefaultAsync(br =>
        br.UserId == int.Parse(userId) &&
        (br.Status == DonateStatus.Pending.ToString() || br.Status == DonateStatus.Approved.ToString()));
        if (existingRequest != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Bạn đã có một yêu cầu mượn laptop đang chờ xử lý hoặc đã được duyệt.");
        }

        var borrowRequest = new BorrowRequest
        {
            UserId = user.UserId,  // Chỉ cần lấy từ token
            ItemId = requestModel.ItemId,
            Status = DonateStatus.Pending.ToString(),
            StartDate = requestModel.StartDate,
            EndDate = requestModel.EndDate,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.BorrowRequest.AddAsync(borrowRequest);
        await _unitOfWork.SaveAsync(); // Lưu lại vào DB
    }



    // Cập nhật yêu cầu mượn
    public async Task UpdateBorrowRequest(string token, int requestId, UpdateBorrowRequestReqModel updateModel)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");

        var borrowRequest = await _unitOfWork.BorrowRequest.GetByIdAsync(requestId);
        if (borrowRequest == null)
            throw new ApiException(HttpStatusCode.NotFound, "Borrow request not found.");

        // Kiểm tra xem laptop đã được mượn chưa
        if (updateModel.Status == DonateStatus.Approved.ToString())
        {
            var isBorrowed = await _unitOfWork.BorrowRequest
                .AnyAsync(br => br.ItemId == borrowRequest.ItemId && br.Status == DonateStatus.Approved.ToString());

            if (isBorrowed)
                throw new ApiException(HttpStatusCode.BadRequest, "Laptop is borrowed.");
        }

        borrowRequest.Status = updateModel.Status;
        _unitOfWork.BorrowRequest.Update(borrowRequest);
        await _unitOfWork.SaveAsync();
    }


    // Xóa yêu cầu mượn
    public async Task DeleteBorrowRequest(string token, int requestId)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");

        var borrowRequest = await _unitOfWork.BorrowRequest.GetByIdAsync(requestId);
        if (borrowRequest == null)
            throw new ApiException(HttpStatusCode.NotFound, "Borrow request not found.");

        _unitOfWork.BorrowRequest.Delete(borrowRequest);
        await _unitOfWork.SaveAsync();
    }
}
