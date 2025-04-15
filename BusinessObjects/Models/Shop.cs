using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public int UserId { get; set; }

    public string ShopName { get; set; }

    public string ShopAddress { get; set; }

    public string ShopPhone { get; set; }

    public string BusinessLicense { get; set; }

    public string BankName { get; set; }

    public string BankNumber { get; set; }

    public string Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; }
}
