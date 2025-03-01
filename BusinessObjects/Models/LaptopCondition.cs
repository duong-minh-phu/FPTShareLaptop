using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class LaptopCondition
{
    public int ConditionId { get; set; }

    public int? BorrowDetailId { get; set; }

    public string? ConditionBefore { get; set; }

    public string? ConditionAfter { get; set; }

    public string? DamageDescription { get; set; }

    public decimal? RepairCost { get; set; }

    public virtual BorrowDetail? BorrowDetail { get; set; }
}
