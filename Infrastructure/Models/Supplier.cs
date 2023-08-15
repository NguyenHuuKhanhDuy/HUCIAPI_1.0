using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Supplier
{
    public Guid Id { get; set; }

    public string? SupplierNumber { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int WardId { get; set; }

    public string WardName { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public Guid CreateUserId { get; set; }

    public string CreateUserName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual Employee CreateUser { get; set; } = null!;

    public virtual Location District { get; set; } = null!;

    public virtual ICollection<Import> Imports { get; set; } = new List<Import>();

    public virtual Location Province { get; set; } = null!;

    public virtual Location Ward { get; set; } = null!;
}
