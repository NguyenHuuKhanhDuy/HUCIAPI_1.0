﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class OrderPaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
