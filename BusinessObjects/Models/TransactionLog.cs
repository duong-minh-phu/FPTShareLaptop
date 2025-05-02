using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TransactionLog
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Note { get; set; }

    public int? ReferenceId { get; set; }

    public string? SourceTable { get; set; }

    public decimal? UsedDepositAmount { get; set; }

    public decimal? ExtraPaymentRequired { get; set; }

    public decimal? RefundAmount { get; set; }

    public virtual User User { get; set; } = null!;
}
