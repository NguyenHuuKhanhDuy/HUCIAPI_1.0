using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Location
{
    public int Id { get; set; }

    public int? ParentId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> EmployeeDistricts { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeProvinces { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> EmployeeWards { get; set; } = new List<Employee>();

    public virtual ICollection<Order> OrderDistricts { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderProvinces { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderWards { get; set; } = new List<Order>();

    public virtual ICollection<Supplier> SupplierDistricts { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierProvinces { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierWards { get; set; } = new List<Supplier>();
}
