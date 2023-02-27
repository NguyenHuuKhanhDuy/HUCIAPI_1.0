using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Brand
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }

    public Guid UserCreateId { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
