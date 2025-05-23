﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class DepositTransaction
{
    public int DepositId { get; set; }

    public int ContractId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime DepositDate { get; set; }

    public virtual ICollection<CompensationTransaction> CompensationTransactions { get; set; } = new List<CompensationTransaction>();

    public virtual BorrowContract Contract { get; set; } = null!;

    public virtual ICollection<RefundTransaction> RefundTransactions { get; set; } = new List<RefundTransaction>();

    public virtual User User { get; set; } = null!;
}
