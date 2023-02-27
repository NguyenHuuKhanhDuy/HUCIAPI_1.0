using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels;

namespace Services.Interface
{
    public interface IEmployeeServices
    {
        Task<EmployeeDto> Login(UserVM userVM, string Key);
    }
}
