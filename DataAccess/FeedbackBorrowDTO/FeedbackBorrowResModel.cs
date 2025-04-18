﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.FeedbackBorrowDTO
{
    public class FeedbackBorrowResModel
    {
        public int FeedbackBorrowId { get; set; } 
        public int BorrowHistoryId { get; set; } 
        public int ItemId { get; set; } 
        public int UserId { get; set; } 
        public DateTime FeedbackDate { get; set; } 
        public int Rating { get; set; } 
        public string Comments { get; set; } 
        public bool IsAnonymous { get; set; } 
    }
}
