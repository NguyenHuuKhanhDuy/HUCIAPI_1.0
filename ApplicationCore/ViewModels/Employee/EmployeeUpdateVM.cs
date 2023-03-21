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

        [RegularExpression(RegexConstants.REGEX_EMAIL, ErrorMessage = EmployeeConstants.INVAILD_EMAIL)]
        public string? Email { get; set; }

        [RegularExpression(RegexConstants.REGEX_PHONE, ErrorMessage = EmployeeConstants.INVAILD_PHONE)]
        public string? Phone { get; set; }

        //[RegularExpression(RegexConstants.REGEX_BIRTHDAY, ErrorMessage = EmployeeConstants.INVAILD_BIRTHDAY)]
        public DateTime? Birthday { get; set; }

        [Range(0, 2, ErrorMessage = EmployeeConstants.INVAILD_GENGER)]
        public int? Gender { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_PROVINCE)]
        public int? ProvinceId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_DISTRICT)]
        public int? DistrictId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_WARD)]
        public int? WardId { get; set; }

        public string? Notes { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = EmployeeConstants.INVAILD_SALARY)]
        public int? Salary { get; set; }

        [Range(0, 1, ErrorMessage = EmployeeConstants.INVAILD_SALARY_TYPE)]
        public int? SalaryTypeId { get; set; }

        [Range(0, 3, ErrorMessage = EmployeeConstants.INVAILD_RULE)]
        public int? RuleId { get; set; }

        public string? Address { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
