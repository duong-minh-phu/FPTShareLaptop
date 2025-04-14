using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class PurchasedLaptop
{
    public int PurchasedLaptopId { get; set; }

    public int DonateItemId { get; set; }

    public int SponsorFundId { get; set; }

    public decimal PurchaseAmount { get; set; }

    public DateTime PurchaseDate { get; set; }

    public virtual DonateItem DonateItem { get; set; } = null!;

    public virtual SponsorFund SponsorFund { get; set; } = null!;
}
