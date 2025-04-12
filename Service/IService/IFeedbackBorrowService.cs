using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.FeedbackBorrowDTO;

namespace Service.IService
{
    public interface IFeedbackBorrowService
    {
        Task<List<FeedbackBorrowResModel>> GetAllFeedbacks(); 
        Task<FeedbackBorrowResModel> GetFeedbackById(int feedbackId); 
        Task<FeedbackBorrowResModel> CreateFeedback(string token, CreateFeedbackBorrowReqModel request); 
        Task UpdateFeedback(int feedbackId, UpdateFeedbackBorrowReqModel request, string token); 
        Task DeleteFeedback(string token, int feedbackId); 
    }
}
