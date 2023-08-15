using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Rule
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
