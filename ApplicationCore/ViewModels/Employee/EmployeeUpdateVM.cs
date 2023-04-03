using Common.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.ViewModels.Employee
{
    public class EmployeeUpdateVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; } = null!;

        [RegularExpression(RegexConstants.REGEX_EMAIL, ErrorMessage = EmployeeConstants.INVALID_EMAIL)]
        public string? Email { get; set; }

        [RegularExpression(RegexConstants.REGEX_PHONE, ErrorMessage = EmployeeConstants.INVALID_PHONE)]
        public string? Phone { get; set; }

        //[RegularExpression(RegexConstants.REGEX_BIRTHDAY, ErrorMessage = EmployeeConstants.INVALID_BIRTHDAY)]
        public DateTime? Birthday { get; set; }

        [Range(0, 2, ErrorMessage = EmployeeConstants.INVALID_GENGER)]
        public int? Gender { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_PROVINCE)]
        public int? ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_DISTRICT)]
        public int? DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_WARD)]
        public int? WardId { get; set; }

        public string? Notes { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVALID_SALARY)]
        public int? Salary { get; set; }

        [Range(0, 1, ErrorMessage = EmployeeConstants.INVALID_SALARY_TYPE)]
        public int? SalaryTypeId { get; set; }

        [Range(0, 3, ErrorMessage = EmployeeConstants.INVALID_RULE)]
        public int? RuleId { get; set; }

        public string? Address { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string Image { get; set; } = null!;

    }
}
