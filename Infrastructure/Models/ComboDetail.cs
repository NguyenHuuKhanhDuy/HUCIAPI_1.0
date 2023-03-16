using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ComboDetail
{
    public Guid Id { get; set; }

    public Guid ComboId { get; set; }

    public Guid ProductId { get; set; }

    public bool IsDelete { get; set; }

    public int Quantity { get; set; }

    public virtual Product Combo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
