﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.FeedbackBorrowDTO
{
    public class CreateFeedbackBorrowReqModel
    {
        public int BorrowHistoryId { get; set; }

        public int ItemId { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }
     
    }
}
