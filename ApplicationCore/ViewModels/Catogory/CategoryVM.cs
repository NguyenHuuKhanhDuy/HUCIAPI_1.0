using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Catogory
{
    public class CategoryVM
    {
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = CategoryConstants.INVALID_CATEGORY_PARENT_ID)]
        public Guid ParentId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVALID_USER_CREATE)]
        public Guid UserCreateId { get; set; }
    }
}
