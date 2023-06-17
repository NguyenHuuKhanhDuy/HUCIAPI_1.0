using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Ip
{
    public Guid Id { get; set; }

    public string Ipv4 { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }

    public string Notes { get; set; } = null!;
}
