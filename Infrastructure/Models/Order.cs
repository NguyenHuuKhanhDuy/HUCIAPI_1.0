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

    public Guid? VoucherId { get; set; }

    public string VoucherName { get; set; } = null!;

    public int VoucherDiscount { get; set; }

    public int OrderDiscount { get; set; }

    public int TotalOrderDiscount { get; set; }

    public int TotalPayment { get; set; }

    public int OrderStatusId { get; set; }

    public string OrderStatusName { get; set; } = null!;

    public int OrderStatusPaymentId { get; set; }

    public string OrderStatusPaymentName { get; set; } = null!;

    public int OrderStatusShipingId { get; set; }

    public string OrderStatusShipingName { get; set; } = null!;

    public int OrderShipingMethodId { get; set; }

    public string OrderShipingMethodName { get; set; } = null!;

    public string OrderNote { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public Guid CreateEmployeeId { get; set; }

    public string CreateEmployeeName { get; set; } = null!;

    public virtual Employee CreateEmployee { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Location District { get; set; } = null!;

    public virtual ShipingMethod OrderShipingMethod { get; set; } = null!;

    public virtual StatusOrder OrderStatus { get; set; } = null!;

    public virtual StatusPayment OrderStatusPayment { get; set; } = null!;

    public virtual StatusShiping OrderStatusShiping { get; set; } = null!;

    public virtual Location Province { get; set; } = null!;

    public virtual Voucher? Voucher { get; set; }

    public virtual Location Ward { get; set; } = null!;
}
