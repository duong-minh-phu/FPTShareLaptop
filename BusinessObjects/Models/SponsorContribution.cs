using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SponsorContribution
{
    public int SponsorContributionId { get; set; }

    public int SponsorFundId { get; set; }

    public int PurchasedLaptopId { get; set; }

    public decimal ContributedAmount { get; set; }

    public virtual PurchasedLaptop PurchasedLaptop { get; set; }

    public virtual SponsorFund SponsorFund { get; set; }
}
