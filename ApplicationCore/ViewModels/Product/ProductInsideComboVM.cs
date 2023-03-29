using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Product
{
    public class ProductInsideComboVM
    {
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = ProductConstants.INVALID_PRODUCT_ID)]
        public Guid ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_QUANTITY)]
        public int Quantity { get; set; }
    }
}
