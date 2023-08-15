using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class VoucherStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
