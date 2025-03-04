using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowHistory
{
    public int HistoryId { get; set; }

    public int? StudentId { get; set; }

    public int? LaptopId { get; set; }

    public int? BorrowDetailId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Status { get; set; }

    public virtual Student? Student { get; set; }
}
