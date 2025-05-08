using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowContract
{
    public int ContractId { get; set; }

    public int RequestId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime ContractDate { get; set; }

    public string Terms { get; set; } = null!;

    public string ConditionBorrow { get; set; } = null!;

    public decimal ItemValue { get; set; }

    public DateTime ExpectedReturnDate { get; set; }

    public virtual ICollection<CompensationTransaction> CompensationTransactions { get; set; } = new List<CompensationTransaction>();

    public virtual ICollection<ContractImage> ContractImages { get; set; } = new List<ContractImage>();

    public virtual ICollection<DepositTransaction> DepositTransactions { get; set; } = new List<DepositTransaction>();

    public virtual DonateItem Item { get; set; } = null!;

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual ICollection<RefundTransaction> RefundTransactions { get; set; } = new List<RefundTransaction>();

    public virtual BorrowRequest Request { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
