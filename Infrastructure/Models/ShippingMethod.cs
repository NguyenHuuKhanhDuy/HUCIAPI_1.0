using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ShippingMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
