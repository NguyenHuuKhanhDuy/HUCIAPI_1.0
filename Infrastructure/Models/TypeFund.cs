using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class TypeFund
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FundDetail> FundDetails { get; set; } = new List<FundDetail>();
}
