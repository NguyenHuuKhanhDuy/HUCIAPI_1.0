using ApplicationCore.ViewModels.Product;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Order
{
    public class OrderVM
    {
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;

        public string CustomerAddress { get; set; } = null!;

        public int ProvinceId { get; set; }

        public int DistrictId { get; set; }

        public int WardId { get; set; }

        public int OrderTotal { get; set; }

        public Guid? VoucherId { get; set; }

        public int OrderDiscount { get; set; }

        public int OrderStatusId { get; set; }

        public int OrderStatusPaymentId { get; set; }

        public int OrderStatusShipingId { get; set; }

        public int OrderShipingMethodId { get; set; }

        public string OrderNote { get; set; } = null!;

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVAILD_USER_CREATE)]
        public Guid CreateEmployeeId { get; set; }

        public List<ProductInsideComboVM> products { get; set; } = new List<ProductInsideComboVM>();
    }
}
