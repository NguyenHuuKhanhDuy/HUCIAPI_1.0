using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ImportDetail
{
    public Guid Id { get; set; }

    public Guid ImportId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductNumber { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int ProductPriceImport { get; set; }

    public int Quantity { get; set; }

    public virtual Import Import { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
