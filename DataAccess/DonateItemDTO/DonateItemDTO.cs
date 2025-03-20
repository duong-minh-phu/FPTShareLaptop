using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DonateItemDTO
{
    public class DonateItemDTO
    {
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Cpu { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? ScreenSize { get; set; }
        public string? ConditionItem { get; set; }
        public int? TotalBorrowedCount { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        public int? DonateFormId { get; set; }
    }
}
