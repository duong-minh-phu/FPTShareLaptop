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

        public FeedbackBorrowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public async Task CreateFeedback(CreateFeedbackBorrowReqModel model, string token)
        {
            var newFeedback = new FeedbackBorrow
            {
                BorrowHistoryId = model.BorrowHistoryId,
                ItemId = model.ItemId,
                UserId = model.UserId,
                FeedbackDate = DateTime.UtcNow,
                Rating = model.Rating,
                Comments = model.Comments,
                IsAnonymous = model.IsAnonymous
            };

            await _unitOfWork.FeedbackBorrow.AddAsync(newFeedback);
            await _unitOfWork.SaveAsync();
        }


        // Cập nhật feedback
        public async Task UpdateFeedback(int feedbackId, UpdateFeedbackBorrowReqModel model,string token)
        {
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
        public async Task DeleteFeedback(int feedbackId)
        {
            var feedback = await _unitOfWork.FeedbackBorrow.GetByIdAsync(feedbackId);
            if (feedback == null)
                throw new ApiException(HttpStatusCode.NotFound, "Feedback not found.");

            _unitOfWork.FeedbackBorrow.Delete(feedback);
            await _unitOfWork.SaveAsync();
        }
    }
}
