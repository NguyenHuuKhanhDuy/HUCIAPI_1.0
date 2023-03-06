using ApplicationCore.ModelsDto;
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

        Task<EmployeeDto> UpdateEmployee(EmployeeUpdateVM employeeVM);
    }
}
