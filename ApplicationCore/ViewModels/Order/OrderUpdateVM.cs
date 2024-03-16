using ApplicationCore.ViewModels.Product;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Order
{
    public class OrderUpdateVM
    {
        [Required]
        public Guid Id { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = OrderConstants.INVALID_VOUCHER_ID)]
        public Guid? VoucherId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_ORDER_STATUS_ID)]
        public int OrderStatusId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_PAYMENT_STATUS_ID)]
        public int OrderStatusPaymentId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_SHIPPING_STATUS_ID)]
        public int OrderStatusShippingId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_SHIPPING_METHOD_ID)]
        public int OrderShippingMethodId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_SOURCE_ORDER_ID)]
        public int OrderSourceId { get; set; }

        public string? OrderNote { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? CustomerAddress { get; set; }

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public long ShippingFee { get; set; }

        public Guid? UserSeparateCommissionId { get; set; }

        public string OrderDescription { get; set; } = null!;

        public bool IsNotCommission { get; set; }

        public List<ProductInsideOrderVM> products { get; set; } = new List<ProductInsideOrderVM>();

    }
}
