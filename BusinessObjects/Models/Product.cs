using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? ShopId { get; set; }

    public int? CategoryId { get; set; }

    public string? ProductName { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? StockQuantity { get; set; }

    public string? Status { get; set; }

    public string? Cpu { get; set; }

    public string? Ram { get; set; }

    public string? Storage { get; set; }

    public string? ScreenSize { get; set; }

    public string? Gpu { get; set; }

    public string? BatteryLife { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Shop? Shop { get; set; }
}
