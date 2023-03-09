using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string ProductNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public int WholesalePrice { get; set; }

    public string Image { get; set; } = null!;

    public int Quantity { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public Guid BrandId { get; set; }

    public Guid CategoryId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int ProductTypeId { get; set; }

    public string ProductTypeName { get; set; } = null!;

    public Guid UserCreateId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
