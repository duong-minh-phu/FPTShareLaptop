using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SponsorFund
{
    public int SponsorFundId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool Status { get; set; }

    public string Note { get; set; }

    public string ProofImageUrl { get; set; }

    public DateTime? TransferDate { get; set; }

    public virtual ICollection<SponsorContribution> SponsorContributions { get; set; } = new List<SponsorContribution>();

    public virtual User User { get; set; }
}
