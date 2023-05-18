using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Shift
{
    public Guid Id { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }
}
