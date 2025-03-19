using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ItemCondition
{
    public int ConditionId { get; set; }

    public int ItemId { get; set; }

    public int ContractId { get; set; }

    public int UserId { get; set; }

    public int BorrowHistoryId { get; set; }

    public int ReportId { get; set; }

    public string ConditionType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string CheckedBy { get; set; } = null!;

    public DateTime CheckedDate { get; set; }

    public DateTime RefundDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual BorrowHistory BorrowHistory { get; set; } = null!;

    public virtual BorrowContract Contract { get; set; } = null!;

    public virtual DonateItem Item { get; set; } = null!;

    public virtual ReportDamage Report { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
