using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public int UserId { get; set; }

    public string ShopName { get; set; } = null!;

    public string ShopAddress { get; set; } = null!;

    public string ShopPhone { get; set; } = null!;

    public string BusinessLicense { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BankNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; } = null!;
}
