using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<DonateItem> DonateItems { get; set; } = new List<DonateItem>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
