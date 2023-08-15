using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderSource
{
    public int Id { get; set; }

    public string SourceName { get; set; } = null!;

    public int PercentCommission { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
