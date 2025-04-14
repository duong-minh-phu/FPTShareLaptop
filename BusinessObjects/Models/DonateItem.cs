using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class DonateItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemImage { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string Cpu { get; set; } = null!;

    public string Ram { get; set; } = null!;

    public string Storage { get; set; } = null!;

    public string ScreenSize { get; set; } = null!;

    public string ConditionItem { get; set; } = null!;

    public int TotalBorrowedCount { get; set; }

    public string Status { get; set; } = null!;

    public int UserId { get; set; }

    public int DonateFormId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string GraphicsCard { get; set; } = null!;

    public string Battery { get; set; } = null!;

    public string Ports { get; set; } = null!;

    public int ProductionYear { get; set; }

    public string OperatingSystem { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual ICollection<BorrowContract> BorrowContracts { get; set; } = new List<BorrowContract>();

    public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();

    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();

    public virtual Category Category { get; set; } = null!;

    public virtual DonateForm DonateForm { get; set; } = null!;

    public virtual ICollection<FeedbackBorrow> FeedbackBorrows { get; set; } = new List<FeedbackBorrow>();

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();

    public virtual ICollection<PurchasedLaptop> PurchasedLaptops { get; set; } = new List<PurchasedLaptop>();

    public virtual ICollection<ReportDamage> ReportDamages { get; set; } = new List<ReportDamage>();

    public virtual User User { get; set; } = null!;
}
