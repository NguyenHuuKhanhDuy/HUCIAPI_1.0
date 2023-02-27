using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels;
using Common.Constants;
using Infrastructure.Models;
using Services.Interface;

namespace Services.Implement
{
    public class EmployeeImp : IEmployeeServices
    {
        private readonly HucidbContext _dbContext;

        public EmployeeImp(HucidbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<EmployeeDto> Login(UserVM userVM, string Key)
        {
            var employee =  _dbContext.Employees.Where(p => p.Username == userVM.Username).FirstOrDefault();

            if(employee == null)
            {
                throw new BusinessException(LoginConstants.USER_NOT_EXIST);
            }

            if(employee.Password != userVM.Passwork)
            {
                throw new BusinessException(LoginConstants.PASSWORD_INCORRECT);
            }
            return null;
        }

        private bool CheckPassword(string passDB, string passInput)
        {

        }

        private string HashPassword(string password)
        {
            return HashPassword(password, 12);
        }


        
    }
}
