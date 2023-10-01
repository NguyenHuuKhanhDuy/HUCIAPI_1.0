using ApplicationCore.ModelsDto.CallTakeCare;
using ApplicationCore.ModelsDto.HistoryAction;
using Infrastructure.Models;

namespace ApplicationCore.ModelsDto.Order
{
    public class OrderDto
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

        public int OrderStatusShippingId { get; set; }

        public string OrderStatusShippingName { get; set; } = null!;

        public int OrderShippingMethodId { get; set; }

        public string OrderShippingMethodName { get; set; } = null!;

        public string OrderNote { get; set; } = null!;

        public Guid CreateEmployeeId { get; set; }

        public string CreateEmployeeName { get; set; } = null!;

        public int OrderSourceId { get; set; }

        public string OrderSourceName { get; set; } = null!;

        public int OrderPaymentMethodId { get; set; }

        public string OrderPaymentMethodName { get; set; } = null!;

        public List<OrderDetailDto> products { get; set; } = new List<OrderDetailDto>();

        public List<HistoryActionDto> History { get; set; } = new List<HistoryActionDto>();

        public List<CallTakeCareDto> CallTakeCares { get; set; } = new List<CallTakeCareDto>();
    }
}
