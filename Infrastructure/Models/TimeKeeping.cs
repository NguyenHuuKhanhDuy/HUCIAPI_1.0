using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class TimeKeeping
{
    public Guid Id { get; set; }

    public Guid UserCreateId { get; set; }

    public Guid UserTimeKeepingId { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Employee UserCreate { get; set; } = null!;

    public virtual Employee UserTimeKeeping { get; set; } = null!;
}
