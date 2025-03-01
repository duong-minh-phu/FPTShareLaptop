using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<CategoryItem> CategoryItems { get; set; } = new List<CategoryItem>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
