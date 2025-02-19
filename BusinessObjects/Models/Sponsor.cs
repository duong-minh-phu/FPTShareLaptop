using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Sponsor
{
    public int SponsorId { get; set; }

    public int? UserId { get; set; }

    public string ContactPhone { get; set; }

    public string ContactEmail { get; set; }

    public string Address { get; set; }

    public virtual ICollection<DonationForm> DonationForms { get; set; } = new List<DonationForm>();

    public virtual User User { get; set; }
}
