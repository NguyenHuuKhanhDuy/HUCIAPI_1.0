using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderDetail
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string ProductImage { get; set; } = null!;

    public int ProductPrice { get; set; }

    public int Discount { get; set; }

    public int SubTotal { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
