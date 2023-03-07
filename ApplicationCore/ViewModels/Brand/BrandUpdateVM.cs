using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Brand
{
    public class BrandUpdateVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = BrandConstants.INVAILD_BRAND_ID)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
