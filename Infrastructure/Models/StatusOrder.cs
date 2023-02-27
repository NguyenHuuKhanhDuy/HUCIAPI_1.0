using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class StatusOrder
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
