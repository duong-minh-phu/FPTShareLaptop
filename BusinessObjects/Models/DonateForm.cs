using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class DonateForm
{
    public int DonateFormId { get; set; }

    public string ItemName { get; set; }

    public string Status { get; set; }

    public string ItemDescription { get; set; }

    public DateTime CreatedDate { get; set; }

    public int UserId { get; set; }

    public int DonateQuantity { get; set; }

    public virtual ICollection<DonateItem> DonateItems { get; set; } = new List<DonateItem>();

    public virtual User? User { get; set; }
}
