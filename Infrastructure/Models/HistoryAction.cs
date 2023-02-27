using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class HistoryAction
{
    public Guid IdAction { get; set; }

    public string Description { get; set; } = null!;

    public Guid UserCreateId { get; set; }

    public string TypeActionName { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int TypeActionId { get; set; }

    public virtual TypeAction TypeAction { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
