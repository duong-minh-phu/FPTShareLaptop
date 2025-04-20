using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ReportDamage
{
    public int ReportId { get; set; }

    public int ItemId { get; set; }

    public int BorrowHistoryId { get; set; }

    public string ImageUrlreport { get; set; } = null!;

    public string Note { get; set; } = null!;

    public string ConditionBeforeBorrow { get; set; } = null!;

    public string ConditionAfterReturn { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public decimal DamageFee { get; set; }

    public virtual BorrowHistory BorrowHistory { get; set; } = null!;

    public virtual ICollection<CompensationTransaction> CompensationTransactions { get; set; } = new List<CompensationTransaction>();

    public virtual DonateItem Item { get; set; } = null!;

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual ICollection<RefundTransaction> RefundTransactions { get; set; } = new List<RefundTransaction>();
}
