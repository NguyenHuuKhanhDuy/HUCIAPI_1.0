using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Promotion
{
    public class PromotionUpdateVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = ProductConstants.INVALID_PRODUCT_ID)]
        public Guid ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_QUANTITY)]
        public int QuantityFrom { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_PRICE)]
        public int Price { get; set; }
    }
}
