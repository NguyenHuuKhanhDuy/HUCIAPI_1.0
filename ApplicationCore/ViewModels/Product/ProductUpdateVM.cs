using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = ProductConstants.INVAILD_PRODUCT_ID)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_PRICE)]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_WHOLESALE_PRICE)]
        public int WholesalePrice { get; set; }

        public string Image { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_WHOLESALE_PRICE)]
        public int Quantity { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = BrandConstants.INVAILD_BRAND_ID)]
        public Guid BrandId { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVAILD_CATEGORY_ID)]
        public Guid CategoryId { get; set; }

        public string Description { get; set; } = null!;
    }
}
