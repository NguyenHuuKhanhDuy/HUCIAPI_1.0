﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class TypeAction
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<HistoryAction> HistoryActions { get; set; } = new List<HistoryAction>();
}
