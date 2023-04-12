using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Promotion
{
    public class PromotionVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = ProductConstants.INVALID_PRODUCT_ID)]
        public Guid ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_QUANTITY)]
        public int QuantityFrom { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_PRICE)]
        public int Price { get; set; }

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVALID_USER_CREATE)]
        public Guid UserCreateId { get; set; }
    }
}
