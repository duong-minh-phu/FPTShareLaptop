using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowDetail
{
    public int BorrowDetailId { get; set; }

    public int? RequestId { get; set; }

    public int? PickupId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    public string ConditionOnReturn { get; set; }

    public string Status { get; set; }

    public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();

    public virtual BorrowRequest Request { get; set; }
}
