using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerAddress { get; set; } = null!;

    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public int WardId { get; set; }

    public string WardName { get; set; } = null!;

    public int OrderTotal { get; set; }

    public int BenefitOrder { get; set; }

    public int OrderDiscount { get; set; }

    public int TotalOrderDiscount { get; set; }

    public int TotalPayment { get; set; }

    public int OrderStatusId { get; set; }

    public string OrderStatusName { get; set; } = null!;

    public string OrderNote { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public Guid CreateEmployeeId { get; set; }

    public string CreateEmployeeName { get; set; } = null!;

    public int OrderSourceId { get; set; }

    public string OrderSourceName { get; set; } = null!;

    public int OrderShippingMethodId { get; set; }

    public string OrderShippingMethodName { get; set; } = null!;

    public bool IsRemovedCallTakeCare { get; set; }

    public DateTime SendDate { get; set; }

    public bool? IsUseExcelFile { get; set; }

    public bool IsUpSale { get; set; }

    public bool IsOrderWholeSale { get; set; }

    public long ShippingFee { get; set; }

    public Guid? UserSeparateCommissionId { get; set; }

    public string OrderDescription { get; set; } = null!;

    public bool IsNotCommission { get; set; }

    public virtual Employee CreateEmployee { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Location District { get; set; } = null!;

    public virtual ICollection<OrderCommission> OrderCommissions { get; set; } = new List<OrderCommission>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ShippingMethod OrderShippingMethod { get; set; } = null!;

    public virtual OrderSource OrderSource { get; set; } = null!;

    public virtual StatusOrder OrderStatus { get; set; } = null!;

    public virtual ICollection<OrderTakeCare> OrderTakeCares { get; set; } = new List<OrderTakeCare>();

    public virtual Location Province { get; set; } = null!;

    public virtual Employee? UserSeparateCommission { get; set; }

    public virtual Location Ward { get; set; } = null!;
}
