using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.BorrowRequestDTO
{
    public class UpdateBorrowRequestReqModel
    {
        public string Status { get; set; } // Duyệt, từ chối, hoặc trả laptop     
    }
}
