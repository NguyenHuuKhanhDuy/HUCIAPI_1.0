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

        public List<ProductInsideOrderVM> products { get; set; } = new List<ProductInsideOrderVM>();

    }
}
