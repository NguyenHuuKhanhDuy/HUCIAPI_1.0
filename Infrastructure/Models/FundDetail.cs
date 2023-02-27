using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class FundDetail
{
    public Guid Id { get; set; }

    public Guid FundId { get; set; }

    public int TypeFundId { get; set; }

    public string TypeFundName { get; set; } = null!;

    public int AmountMoney { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid UserCreateId { get; set; }

    public string Note { get; set; } = null!;

    public virtual Fund Fund { get; set; } = null!;

    public virtual TypeFund TypeFund { get; set; } = null!;

    public virtual Employee UserCreate { get; set; } = null!;
}
