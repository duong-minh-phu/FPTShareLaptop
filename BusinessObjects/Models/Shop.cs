using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public string? TaxCode { get; set; }

    public int? UserId { get; set; }

    public string? ShopName { get; set; }

    public string? ShopLocation { get; set; }

    public string? ContactInfo { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User? User { get; set; }
}
