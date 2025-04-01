using System.Net;
using DataAccess.FeedbackBorrowDTO;
using BusinessObjects.Models;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class FeedbackBorrowService : IFeedbackBorrowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;

        public FeedbackBorrowService(IUnitOfWork unitOfWork, IJWTService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        // Lấy tất cả feedbacks
        public async Task<List<FeedbackBorrowResModel>> GetAllFeedbacks()
        {
            var feedbacks = await _unitOfWork.FeedbackBorrow.GetAllAsync();
            return feedbacks.Select(f => new FeedbackBorrowResModel
            {
                FeedbackBorrowId = f.FeedbackBorrowId,
                BorrowHistoryId = f.BorrowHistoryId,
                ItemId = f.ItemId,
                UserId = f.UserId,
                FeedbackDate = f.FeedbackDate,
                Rating = f.Rating,
                Comments = f.Comments,
                IsAnonymous = f.IsAnonymous
            }).ToList();
        }

        // Lấy feedback theo ID
        public async Task<FeedbackBorrowResModel> GetFeedbackById(int feedbackId)
        {
            var feedback = await _unitOfWork.FeedbackBorrow.GetByIdAsync(feedbackId);
            if (feedback == null)
                throw new ApiException(HttpStatusCode.NotFound, "Feedback not found.");

            return new FeedbackBorrowResModel
            {
                FeedbackBorrowId = feedback.FeedbackBorrowId,
                BorrowHistoryId = feedback.BorrowHistoryId,
                ItemId = feedback.ItemId,
                UserId = feedback.UserId,
                FeedbackDate = feedback.FeedbackDate,
                Rating = feedback.Rating,
                Comments = feedback.Comments,
                IsAnonymous = feedback.IsAnonymous
            };
        }

        // Tạo feedback mới
        public async Task CreateFeedback(string token, CreateFeedbackBorrowReqModel model)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var borrowHistory = await _unitOfWork.BorrowHistory.GetByIdAsync(model.BorrowHistoryId);
            if (borrowHistory == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid BorrowHistoryId.");

            var item = await _unitOfWork.DonateItem.GetByIdAsync(model.ItemId);
            if (item == null)
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid ItemId.");

            var newFeedback = new FeedbackBorrow
            {
                BorrowHistoryId = model.BorrowHistoryId,
                ItemId = model.ItemId,
                UserId = user.UserId,
                FeedbackDate = DateTime.UtcNow,
                Rating = model.Rating,
                Comments = model.Comments,
                IsAnonymous = false
            };

            await _unitOfWork.FeedbackBorrow.AddAsync(newFeedback);
            await _unitOfWork.SaveAsync();
        }


        // Cập nhật feedback
        public async Task UpdateFeedback(int feedbackId, UpdateFeedbackBorrowReqModel model,string token)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var feedback = await _unitOfWork.FeedbackBorrow.GetByIdAsync(feedbackId);
            if (feedback == null)
                throw new ApiException(HttpStatusCode.NotFound, "Feedback not found.");

            feedback.Rating = model.Rating;
            feedback.Comments = model.Comments;
            feedback.IsAnonymous = model.IsAnonymous;

            _unitOfWork.FeedbackBorrow.Update(feedback);
            await _unitOfWork.SaveAsync();
        }

        // Xóa feedback
        public async Task DeleteFeedback(string token ,int feedbackId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var feedback = await _unitOfWork.FeedbackBorrow.GetByIdAsync(feedbackId);
            if (feedback == null)
                throw new ApiException(HttpStatusCode.NotFound, "Feedback not found.");

            _unitOfWork.FeedbackBorrow.Delete(feedback);
            await _unitOfWork.SaveAsync();
        }
    }
}
