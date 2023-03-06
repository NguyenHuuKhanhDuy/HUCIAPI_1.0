using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class SalaryType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
