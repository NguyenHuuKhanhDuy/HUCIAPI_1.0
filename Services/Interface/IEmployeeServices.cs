using ApplicationCore.ModelsDto;
using ApplicationCore.ModelsDto.Employee;
using ApplicationCore.ViewModels.Employee;
using Infrastructure.Models;

namespace Services.Interface
{
    public interface IEmployeeServices
    {
        Task<EmployeeDto> Login(UserVM userVM);

        Task<EmployeeDto> CreateEmployeeAsync(EmployeeVM employeeVM);

        Task<List<EmployeeDto>> GetAllEmployeeAsync();

        Task<EmployeeDto> GetEmployeeByIdAsync(Guid idEmployee);

        Task<EmployeeDto> UpdateEmployeeAsync(EmployeeUpdateVM employeeVM);

        Task DeleteEmployeeByIdAsync(Guid employeeId);

        Task<DataForCreateEmployeeDto> DataForCreateEmployeeAsync();

        Task<SalaryEmployeeDto> SalaryEmployeeByIdAsync(Guid idEmployee, DateTime startDate, DateTime endDate);

        string GetPassworkEncrypt(string password);

        Task<BenefitDto> CalculateBenefitWithDateAsync(DateTime startDate, DateTime endDate);
        Task<List<ReportMonthDto>> ReportMonthsAgo(int monthsAgo);
        Task<List<RoleDto>> GetAllRolesAsync();
    }
}
