using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
