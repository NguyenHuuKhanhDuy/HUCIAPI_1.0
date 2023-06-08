using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderPaymentMethod
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
