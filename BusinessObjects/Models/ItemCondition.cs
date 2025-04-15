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

    public string ConditionType { get; set; }

    public string Description { get; set; }

    public string ImageUrl { get; set; }

    public string CheckedBy { get; set; }

    public DateTime CheckedDate { get; set; }

    public DateTime RefundDate { get; set; }

    public string Status { get; set; }

    public virtual BorrowHistory BorrowHistory { get; set; }

    public virtual BorrowContract Contract { get; set; }

    public virtual DonateItem Item { get; set; }

    public virtual ReportDamage Report { get; set; }

    public virtual User User { get; set; }
}
