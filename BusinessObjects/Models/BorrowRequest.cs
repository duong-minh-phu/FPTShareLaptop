using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowRequest
{
    public int RequestId { get; set; }

    public int? StudentId { get; set; }

    public int? SponsorLaptopId { get; set; }

    public DateTime? RequestDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? RequestStatus { get; set; }

    public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();

    public virtual Student? Student { get; set; }
}
