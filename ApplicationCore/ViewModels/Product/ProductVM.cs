using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Product
{
    public class ProductVM
    {
        [Required]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_PRICE)]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_WHOLESALE_PRICE)]
        public int WholesalePrice { get; set; }

        public string Image { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_WHOLESALE_PRICE)]
        public int OnHand { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = BrandConstants.INVAILD_BRAND_ID)]
        public Guid BrandId { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVAILD_CATEGORY_ID)]
        public Guid CategoryId { get; set; }

        public string Description { get; set; } = null!;

        [Range(0, 1, ErrorMessage = ProductConstants.INVALID_PRODUCT_TYPE)]
        public int ProductTypeId { get; set; }

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVAILD_USER_CREATE)]
        public Guid UserCreateId { get; set; }
    }
}
