﻿using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = ProductConstants.INVALID_PRODUCT_ID)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int SalePrice { get; set; }

        public int NormalPrice { get; set; }

        public int WholesalePrice { get; set; }

        public int OriginalPrice { get; set; }

        public string Image { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = ProductConstants.INVALID_QUANTITY)]
        public int Quantity { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = BrandConstants.INVALID_BRAND_ID)]
        public Guid BrandId { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVALID_CATEGORY_ID)]
        public Guid CategoryId { get; set; }

        public string Description { get; set; } = null!;
    }
}
