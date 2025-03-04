using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Commitment
{
    public int CommitmentId { get; set; }

    public int? StudentId { get; set; }

    public int? SponsorId { get; set; }

    public string? CommitmentText { get; set; }

    public DateTime? SignedDate { get; set; }

    public string? Status { get; set; }

    public virtual Sponsor? Sponsor { get; set; }

    public virtual Student? Student { get; set; }
}
