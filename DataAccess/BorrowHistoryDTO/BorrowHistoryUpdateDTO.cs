using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BorrowHistoryDTO
{
    public class BorrowHistoryUpdateDTO
    {
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
