using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class CompensationTransaction
{
    public int CompensationId { get; set; }

    public int ContractId { get; set; }

    public int UserId { get; set; }

    public int ReportDamageId { get; set; }

    public int DepositTransactionId { get; set; }

    public decimal CompensationAmount { get; set; }

    public decimal UsedDepositAmount { get; set; }

    public decimal ExtraPaymentRequired { get; set; }

    public string Status { get; set; } = null!;

    public virtual BorrowContract Contract { get; set; } = null!;

    public virtual DepositTransaction DepositTransaction { get; set; } = null!;

    public virtual ReportDamage ReportDamage { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
