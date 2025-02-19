using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CategoryItem
{
    public int CategoryItemId { get; set; }

    public string CategoryItemName { get; set; }

    public virtual ICollection<SponsorItem> SponsorItems { get; set; } = new List<SponsorItem>();
}
