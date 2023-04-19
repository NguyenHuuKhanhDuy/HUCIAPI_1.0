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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public async Task DeleteFundAsync(Guid fundId)
        {
            var fund = await FindFundAsync(fundId);

            fund.IsDeleted = true;
            fund.Name += BaseConstants.DELETE;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        public async Task<FundDto> UpdateFundAsync(FundUpdateVM fundVM)
        {
            var fund = await FindFundAsync(fundVM?.Id);

            MapFFundUpdateVMTFund(fund, fundVM);
            await _dbContext.SaveChangesAsync();

            var dto = MapFFundTFundDto(fund);
            
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<Fund> FindFundAsync(Guid? fundId)
        {
            if(fundId == null || fundId == Guid.Empty)
            {
                throw new BusinessException(FundConstants.FUND_NOT_EXIST);
            }

            var fund = await _dbContext.Funds.FirstOrDefaultAsync(x => x.Id == fundId && !x.IsDeleted);

            if(fund == null)
            {
                throw new BusinessException(FundConstants.FUND_NOT_EXIST);
            }

            return fund;
        }
    }
}
