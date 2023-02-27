using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Fund
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int TotalFund { get; set; }

    public DateTime CreateDate { get; set; }

    public bool? IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public Guid UserCreateId { get; set; }

    public string Note { get; set; } = null!;

    public virtual ICollection<FundDetail> FundDetails { get; } = new List<FundDetail>();

    public virtual Employee UserCreate { get; set; } = null!;
}
