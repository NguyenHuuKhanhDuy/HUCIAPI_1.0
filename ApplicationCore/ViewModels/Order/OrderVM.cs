﻿using ApplicationCore.ViewModels.Product;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Order
{
    public class OrderVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = OrderConstants.INVALID_CUSTOMER_ID)]
        public Guid CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;

        public string CustomerAddress { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_PROVINCE)]
        public int ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_DISTRICT)]
        public int DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_WARD)]
        public int WardId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_ORDER_DISCOUNT)]
        public int OrderDiscount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_ORDER_STATUS_ID)]
        public int OrderStatusId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_SHIPPING_METHOD_ID)]
        public int OrderShippingMethodId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = OrderConstants.INVALID_SOURCE_ORDER_ID)]
        public int OrderSourceId { get; set; }

        public DateTime OrderDate { get; set; }

        public string? OrderNote { get; set; }

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVALID_USER_CREATE)]
        public Guid CreateEmployeeId { get; set; }

        public long ShippingFee { get; set; }

        public Guid? UserSeparateCommissionId { get; set; }

        public string OrderDescription { get; set; } = null!;

        public bool IsNotCommission { get; set; }

        public List<ProductInsideOrderVM> products { get; set; } = new List<ProductInsideOrderVM>();
    }
}
