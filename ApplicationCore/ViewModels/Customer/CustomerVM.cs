using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Customer
{
    public class CustomerVM
    {
        [Required]
        public string Name { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_EMAIL, ErrorMessage = EmployeeConstants.INVAILD_EMAIL)]
        public string Email { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_PHONE, ErrorMessage = EmployeeConstants.INVAILD_PHONE)]
        public string Phone { get; set; } = null!;

        public DateTime Birthday { get; set; }

        [Range(0, 2, ErrorMessage = EmployeeConstants.INVAILD_GENGER)]
        public int Gender { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_PROVINCE)]
        public int ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_DISTRICT)]
        public int DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_WARD)]
        public int WardId { get; set; }

        public string Notes { get; set; } = null!;

        [Required]
        [RegularExpression(RegexConstants.REGEX_GUID, ErrorMessage = EmployeeConstants.INVAILD_USER_CREATE)]
        public Guid CreateUserId { get; set; }

        public string IpV4 { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
