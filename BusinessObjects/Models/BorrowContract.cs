using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowContract
{
    public int ContractId { get; set; }

    public int RequestId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; }

    public DateTime ContractDate { get; set; }

    public string Terms { get; set; }

    public string ConditionBorrow { get; set; }

    public decimal ItemValue { get; set; }

    public DateTime ExpectedReturnDate { get; set; }

    public virtual ICollection<CompensationTransaction> CompensationTransactions { get; set; } = new List<CompensationTransaction>();

    public virtual ICollection<ContractImage> ContractImages { get; set; } = new List<ContractImage>();

    public virtual ICollection<DepositTransaction> DepositTransactions { get; set; } = new List<DepositTransaction>();

    public virtual DonateItem Item { get; set; }

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual BorrowRequest Request { get; set; }

    public virtual User User { get; set; }
}
