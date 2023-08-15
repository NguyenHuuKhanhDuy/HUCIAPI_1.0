using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid UserCreateId { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
