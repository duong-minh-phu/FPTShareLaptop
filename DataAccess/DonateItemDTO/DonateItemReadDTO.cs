using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DonateItemDTO
{
    public class DonateItemDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string ItemImage { get; set; } = null!; // Chỉ lưu URL ảnh
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Cpu { get; set; } = null!;
        public string Ram { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string ScreenSize { get; set; } = null!;
        public string ConditionItem { get; set; } = null!;
        public int TotalBorrowedCount { get; set; }
        public string Status { get; set; } = null!;
        public int UserId { get; set; }
        public int DonateFormId { get; set; }
    }
}
