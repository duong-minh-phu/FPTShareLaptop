using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string ImageProduct { get; set; } = null!;

    public string ScreenSize { get; set; } = null!;

    public string Storage { get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Ram { get; set; } = null!;

    public string Cpu { get; set; } = null!;

    public int CategoryId { get; set; }

    public int ShopId { get; set; }

    public string Model { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string GraphicsCard { get; set; } = null!;

    public string Battery { get; set; } = null!;

    public string Ports { get; set; } = null!;

    public int ProductionYear { get; set; }

    public string OperatingSystem { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<FeedbackProduct> FeedbackProducts { get; set; } = new List<FeedbackProduct>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual Shop Shop { get; set; } = null!;
}
