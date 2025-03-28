using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.OrderDTO
{
    public class OrderReadDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderAddress { get; set; } = null!;
        public string Field { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
