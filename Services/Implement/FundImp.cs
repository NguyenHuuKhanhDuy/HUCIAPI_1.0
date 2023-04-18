using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Fund;
using ApplicationCore.ViewModels.Fund;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class FundImp : BaseServices, IFundServices
    {
        private readonly HucidbContext _dbContext;
        public FundImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FundDto> CreateFundAsync(FundVM fundVM)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == fundVM.UserCreateId && !x.IsDeleted);

            if(employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var fund = MapFFundVMTFund(fundVM);

            await _dbContext.AddAsync(fund);
            await _dbContext.SaveChangesAsync();

            var fundDto = MapFFundTFundDto(fund);
            fundDto.UserCreateName = employee.Name;

            return fundDto;
        }
    }
}
