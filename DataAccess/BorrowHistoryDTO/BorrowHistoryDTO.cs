using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BorrowHistoryDTO
{
    public class BorrowHistoryDTO
    {
        public int BorrowHistoryId { get; set; }
        public int? RequestId { get; set; }
        public int? ItemId { get; set; }
        public int? UserId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
