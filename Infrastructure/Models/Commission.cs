using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Commission
{
    public Guid Id { get; set; }

    public int TotalPriceFrom { get; set; }

    public int CommissionPrice { get; set; }

    public bool IsDelete { get; set; }

    public Guid UserCreateId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Employee UserCreate { get; set; } = null!;
}
