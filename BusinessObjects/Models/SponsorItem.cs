using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SponsorItem
{
    public int ItemId { get; set; }

    public int? DonationFormId { get; set; }

    public int? CategoryItemId { get; set; }

    public string? ItemName { get; set; }

    public string? Specifications { get; set; }

    public int? QuantityAvailable { get; set; }

    public int? TotalBorrows { get; set; }

    public DateTime? LastCheckedAt { get; set; }

    public string? CurrentStatus { get; set; }

    public string? Condition { get; set; }

    public string? Cpu { get; set; }

    public string? Ram { get; set; }

    public string? Storage { get; set; }

    public string? ScreenSize { get; set; }

    public string? Gpu { get; set; }

    public string? BatteryLife { get; set; }

    public virtual CategoryItem? CategoryItem { get; set; }

    public virtual DonationForm? DonationForm { get; set; }

    public virtual ICollection<SponsorItemImage> SponsorItemImages { get; set; } = new List<SponsorItemImage>();
}
