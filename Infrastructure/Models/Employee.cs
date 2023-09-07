using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public int Gender { get; set; }

    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int WardId { get; set; }

    public string WardName { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int Salary { get; set; }

    public int SalaryTypeId { get; set; }

    public int RuleId { get; set; }

    public string RuleName { get; set; } = null!;

    public Guid CreateUserId { get; set; }

    public string? Address { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Commission> Commissions { get; set; } = new List<Commission>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Location District { get; set; } = null!;

    public virtual ICollection<FundDetail> FundDetails { get; set; } = new List<FundDetail>();

    public virtual ICollection<Fund> Funds { get; set; } = new List<Fund>();

    public virtual ICollection<HistoryAction> HistoryActions { get; set; } = new List<HistoryAction>();

    public virtual ICollection<Import> Imports { get; set; } = new List<Import>();

    public virtual ICollection<OrderCommission> OrderCommissions { get; set; } = new List<OrderCommission>();

    public virtual ICollection<OrderTakeCare> OrderTakeCares { get; set; } = new List<OrderTakeCare>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<OtherCost> OtherCosts { get; set; } = new List<OtherCost>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual Location Province { get; set; } = null!;

    public virtual Rule Rule { get; set; } = null!;

    public virtual SalaryType SalaryType { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<TimeKeeping> TimeKeepingUserCreates { get; set; } = new List<TimeKeeping>();

    public virtual ICollection<TimeKeeping> TimeKeepingUserTimeKeepings { get; set; } = new List<TimeKeeping>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    public virtual Location Ward { get; set; } = null!;
}
