using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SponsorItemImage
{
    public int ImageId { get; set; }

    public int? ItemId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual SponsorItem? Item { get; set; }
}
