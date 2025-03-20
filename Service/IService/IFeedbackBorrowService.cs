﻿using System;
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
        Task CreateFeedback(CreateFeedbackBorrowReqModel request, string token); 
        Task UpdateFeedback(int feedbackId, UpdateFeedbackBorrowReqModel request, string token); 
        Task DeleteFeedback(int feedbackId); 
    }
}
