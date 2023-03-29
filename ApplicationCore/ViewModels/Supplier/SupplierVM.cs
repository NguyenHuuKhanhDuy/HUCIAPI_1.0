using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Supplier
{
    public class SupplierVM
    {
        [Required]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_PROVINCE)]
        public int ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_DISTRICT)]
        public int DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_WARD)]
        public int WardId { get; set; }

        public string Notes { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_EMAIL, ErrorMessage = EmployeeConstants.INVALID_EMAIL)]
        public string Email { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_PHONE, ErrorMessage = EmployeeConstants.INVALID_PHONE)]
        public string Phone { get; set; } = null!;

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVALID_USER_CREATE)]
        public Guid CreateUserId { get; set; }

        public string Address { get; set; } = null!;
    }
}
