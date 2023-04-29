using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderCommission
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid EmployeeId { get; set; }

    public int OrderTotal { get; set; }

    public int OrderCommission1 { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
