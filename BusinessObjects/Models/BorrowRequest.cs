using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int MajorId { get; set; }

    public virtual ICollection<BorrowContract> BorrowContracts { get; set; } = new List<BorrowContract>();

    public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();

    public virtual DonateItem Item { get; set; } = null!;

    public virtual Major Major { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
