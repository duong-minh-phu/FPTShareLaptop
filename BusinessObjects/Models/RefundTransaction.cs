using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class RefundTransaction
{
    public int RefundTransactionId { get; set; }

    public int UserId { get; set; }

    public int DepositId { get; set; }

    public decimal RefundAmount { get; set; }

    public string? RefundNote { get; set; }

    public DateTime RefundDate { get; set; }

    public string Status { get; set; } = null!;

    public int ContractId { get; set; }

    public int ReportId { get; set; }

    public virtual BorrowContract Contract { get; set; } = null!;

    public virtual DepositTransaction Deposit { get; set; } = null!;

    public virtual ReportDamage Report { get; set; } = null!;
}
