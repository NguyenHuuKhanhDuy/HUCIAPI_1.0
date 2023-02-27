using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class StatusImport
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Import> Imports { get; } = new List<Import>();
}
