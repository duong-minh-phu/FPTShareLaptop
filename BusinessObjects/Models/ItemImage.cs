using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ItemImage
{
    public int ItemImageId { get; set; }

    public int ItemId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual DonateItem Item { get; set; } = null!;
}
