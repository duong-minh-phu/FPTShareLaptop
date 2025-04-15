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

    public string Status { get; set; }

    public virtual BorrowContract Contract { get; set; }

    public virtual DepositTransaction DepositTransaction { get; set; }

    public virtual ReportDamage ReportDamage { get; set; }

    public virtual User User { get; set; }
}
