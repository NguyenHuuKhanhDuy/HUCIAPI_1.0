using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Brand
{
    public class BrandVM
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVAILD_USER_CREATE)]
        public Guid UserCreateId { get; set; }
    }
}
