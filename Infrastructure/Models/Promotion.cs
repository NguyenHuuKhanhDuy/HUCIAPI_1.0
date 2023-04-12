using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Promotion
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int QuantityFrom { get; set; }

    public int Price { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid UserCreateId { get; set; }

    public string UserCreateName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
