using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Customer
{
    public class CustomerUpdateVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_EMAIL, ErrorMessage = EmployeeConstants.INVALID_EMAIL)]
        public string Email { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_PHONE, ErrorMessage = EmployeeConstants.INVALID_PHONE)]
        public string Phone { get; set; } = null!;

        public DateTime Birthday { get; set; }

        [Range(0, 2, ErrorMessage = EmployeeConstants.INVALID_GENGER)]
        public int Gender { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_PROVINCE)]
        public int ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_DISTRICT)]
        public int DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_WARD)]
        public int WardId { get; set; }

        public string Notes { get; set; } = null!;

        public string IpV4 { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
