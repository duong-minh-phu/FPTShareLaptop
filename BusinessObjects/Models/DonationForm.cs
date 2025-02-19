using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class DonationForm
{
    public int DonationFormId { get; set; }

    public int? SponsorId { get; set; }

    public string ItemName { get; set; }

    public string ItemDescription { get; set; }

    public int? Quantity { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Sponsor Sponsor { get; set; }

    public virtual ICollection<SponsorItem> SponsorItems { get; set; } = new List<SponsorItem>();
}
