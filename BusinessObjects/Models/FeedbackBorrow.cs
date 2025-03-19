using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FeedbackBorrow
{
    public int FeedbackBorrowId { get; set; }

    public int BorrowHistoryId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public DateTime FeedbackDate { get; set; }

    public int? Rating { get; set; }

    public string Comments { get; set; } = null!;

    public bool IsAnonymous { get; set; }

    public virtual BorrowHistory BorrowHistory { get; set; } = null!;

    public virtual DonateItem Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
