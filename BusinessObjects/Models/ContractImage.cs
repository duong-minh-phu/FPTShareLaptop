using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class ContractImage
{
    public int ContractImageId { get; set; }

    public int BorrowContractId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual BorrowContract BorrowContract { get; set; } = null!;
}
