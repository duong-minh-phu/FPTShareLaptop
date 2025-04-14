using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SponsorFund
{
    public int SponsorFundId { get; set; }

    public int SponsorId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime TransferDate { get; set; }

    public string? ProofImageUrl { get; set; }

    public virtual ICollection<PurchasedLaptop> PurchasedLaptops { get; set; } = new List<PurchasedLaptop>();

    public virtual User Sponsor { get; set; } = null!;
}
