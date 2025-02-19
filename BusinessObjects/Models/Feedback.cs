using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? StudentId { get; set; }

    public int? LaptopId { get; set; }

    public int? HistoryId { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? Rating { get; set; }

    public string Comments { get; set; }

    public virtual BorrowHistory History { get; set; }

    public virtual Student Student { get; set; }
}
