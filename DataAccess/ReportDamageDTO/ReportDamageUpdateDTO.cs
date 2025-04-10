using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ReportDamageDTO
{
    public class ReportDamageUpdateDTO
    {
        public int? ReportId { get; set; }
        public int? ItemId { get; set; }
        public int? BorrowHistoryId { get; set; }
        public string? Note { get; set; }
        public string? ConditionBeforeBorrow { get; set; }
        public string? ConditionAfterReturn { get; set; }
        public decimal? DamageFee { get; set; }
    }
}
