using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowHistory
{
    public int BorrowHistoryId { get; set; }

    public int RequestId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime ReturnDate { get; set; }

    public virtual ICollection<FeedbackBorrow> FeedbackBorrows { get; set; } = new List<FeedbackBorrow>();

    public virtual DonateItem Item { get; set; }

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual ICollection<ReportDamage> ReportDamages { get; set; } = new List<ReportDamage>();

    public virtual BorrowRequest Request { get; set; }

    public virtual User User { get; set; }
}
