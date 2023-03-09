using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Catogory
{
    public class CategoryUpdateVM
    {
        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVAILD_CATEGORY_ID)]
        public Guid Id { get; set; }

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVAILD_CATEGORY_PARENT_ID)]
        public Guid ParentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
