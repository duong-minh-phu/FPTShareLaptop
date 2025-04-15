using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class PurchasedLaptop
{
    public int PurchasedLaptopId { get; set; }

    public int ItemId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime PurchasedDate { get; set; }

    public string InvoiceImageUrl { get; set; }

    public virtual DonateItem Item { get; set; }

    public virtual ICollection<SponsorContribution> SponsorContributions { get; set; } = new List<SponsorContribution>();
}
