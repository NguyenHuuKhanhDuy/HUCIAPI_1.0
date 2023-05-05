using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderTakeCare
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid UserCreateId { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
