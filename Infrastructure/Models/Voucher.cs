using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Voucher
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int DiscountPercent { get; set; }

    public int DiscountPrice { get; set; }

    public int DiscountMax { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int VoucherStatusId { get; set; }

    public string VoucherStatusName { get; set; } = null!;

    public int Used { get; set; }

    public int Quantity { get; set; }

    public bool? IsActive { get; set; }

    public Guid CreateUserId { get; set; }

    public string CreateUserName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string Note { get; set; } = null!;

    public virtual Employee CreateUser { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual VoucherStatus VoucherStatus { get; set; } = null!;
}
