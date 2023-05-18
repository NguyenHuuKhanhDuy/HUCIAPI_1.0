using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OtherCost
{
    public Guid Id { get; set; }

    public int Price { get; set; }

    public Guid UserCreateId { get; set; }

    public DateTime CreateDate { get; set; }

    public string Notes { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual Employee UserCreate { get; set; } = null!;
}
