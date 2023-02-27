using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Location
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> EmployeeDistricts { get; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeProvinces { get; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeWards { get; } = new List<Employee>();

    public virtual ICollection<Order> OrderDistricts { get; } = new List<Order>();

    public virtual ICollection<Order> OrderProvinces { get; } = new List<Order>();

    public virtual ICollection<Order> OrderWards { get; } = new List<Order>();

    public virtual ICollection<Supplier> SupplierDistricts { get; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierProvinces { get; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierWards { get; } = new List<Supplier>();
}
