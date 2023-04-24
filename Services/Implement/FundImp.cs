using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Fund;
using ApplicationCore.ViewModels.Employee;
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
            await _dbContext.Funds.AddAsync(fund);
            await _dbContext.SaveChangesAsync();

            var fundDto = MapFFundTFundDto(fund);
            fundDto.UserCreateName = employee.Name;

            var typeFund = await _dbContext.TypeFunds.FirstOrDefaultAsync(x => x.Id == FundConstants.COLLECT);

            if (typeFund == null)
                return fundDto;

            var fundDetailVM = new FundDetailVM
            {
                FundId = fund.Id,
                TypeFundId = typeFund.Id,
                TypeFundName = typeFund.Name,
                AmountMoney = fundVM.TotalFund,
                Note = $"Create Fund with total money is: {fund.TotalFund}",
                UserCreateId = fundVM.UserCreateId
            };

            var fundDetailDto = await CreateFundDetailAsync(fundDetailVM);
            fundDetailDto.UserCreateName = employee.Name;
            fundDto.FundDetails.Add(fundDetailDto);
            
            return fundDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<FundDetailDto> CreateFundDetailAsync(FundDetailVM fundVM)
        {
            await CheckInforFundDetail(fundVM);

            var fundDetail = MapFFundDetailVMTFundDetail(fundVM);
            await _dbContext.FundDetails.AddAsync(fundDetail);
            await _dbContext.SaveChangesAsync();

            var dto = MapFFundDetailTFundDetailDto(fundDetail);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task CheckInforFundDetail(FundDetailVM fundVM)
        {
            var fund = await _dbContext.Funds.FirstOrDefaultAsync(x => x.Id == fundVM.FundId && !x.IsDeleted);

            if (fund == null)
            {
                throw new BusinessException(FundConstants.FUND_NOT_EXIST);
            }

            var typeFund = await _dbContext.TypeFunds.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fundVM.TypeFundId);

            if (typeFund == null)
            {
                throw new BusinessException(FundConstants.TYPE_FUND_NOT_EXIST);
            }

            if (typeFund.Id == FundConstants.COLLECT)
            {
                fund.TotalFund += fundVM.AmountMoney;
            }
            else if(typeFund.Id == FundConstants.PAY_OUT)
            {
                fund.TotalFund -= fundVM.AmountMoney;
            }
                
            fundVM.TypeFundName = typeFund.Name;

            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fundVM.UserCreateId && !x.IsDeleted);

            if (employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }
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
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == fundVM.UserUpdateId && !x.IsDeleted);

            if(employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var fundDetailVM = new FundDetailVM();
            bool isChangeTotalFund = fund.TotalFund != fundVM?.TotalFund;
            if (isChangeTotalFund)
            {
                int typeFundId = IsCollectFund(fund.TotalFund, fundVM.TotalFund) ? FundConstants.COLLECT : FundConstants.PAY_OUT;
                var typeFund = await _dbContext.TypeFunds.FirstOrDefaultAsync(x => x.Id == typeFundId);

                if (typeFund == null)
                {
                    throw new BusinessException(FundConstants.TYPE_FUND_NOT_EXIST);
                }

                fundDetailVM = new FundDetailVM
                {
                    FundId = fund.Id,
                    TypeFundId = typeFund.Id,
                    TypeFundName = typeFund.Name,
                    AmountMoney = Math.Abs(fund.TotalFund - fundVM.TotalFund),
                    Note = $"Update Fund with total money from {fund.TotalFund} to {fundVM.TotalFund}",
                    UserCreateId = fundVM.UserUpdateId
                };
            }

            MapFFundUpdateVMTFund(fund, fundVM);

            var fundDto = MapFFundTFundDto(fund);

            if (isChangeTotalFund)
            {
                var fundDetailDto = await CreateFundDetailAsync(fundDetailVM);
                fundDetailDto.UserCreateName = employee.Name;
                fundDto.FundDetails.Add(fundDetailDto);
            }

            await _dbContext.SaveChangesAsync();
            return fundDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalFund"></param>
        /// <param name="totalFundUpdate"></param>
        /// <returns></returns>
        private bool IsCollectFund(int totalFund, int totalFundUpdate)
        {
            int difference = totalFund - totalFundUpdate;

            return difference < 0 ? true : false;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        public async Task<FundDto> GetFundByIdAsync(Guid fundId)
        {
            var fund = await FindFundAsync(fundId);
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == fund.UserCreateId);

            var fundDto = MapFFundTFundDto(fund);
            fundDto.UserCreateName = employee.Name;
            fundDto.FundDetails = await GetFundDetailDtoAsync(fundDto.Id);
            
            return fundDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        private async Task<List<FundDetailDto>> GetFundDetailDtoAsync(Guid fundId)
        {
            var fundDetails = await _dbContext.FundDetails.AsNoTracking().Where(x => x.FundId == fundId).OrderByDescending(x => x.CreateDate).ToListAsync();
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var fundDetailsDto = new List<FundDetailDto>();

            foreach (var fundDetail in fundDetails)
            {
                fundDetailsDto.Add(MapFFundDetailTFundDetailDto(fundDetail));
            }

            foreach (var fundDetailDto in fundDetailsDto)
            {
                fundDetailDto.UserCreateName = employees.FirstOrDefault(x => x.Id == fundDetailDto.UserCreateId)?.Name;
            }

            return fundDetailsDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<FundDto>> GetAllFundsAsync()
        {
            var funds = await _dbContext.Funds.Where(x => !x.IsDeleted).ToListAsync();
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var fundDtos = new List<FundDto>();

            foreach (var fund in funds)
            {
                fundDtos.Add(MapFFundTFundDto(fund));
            }

            foreach (var fundDto in fundDtos)
            {
                fundDto.FundDetails = await GetFundDetailDtoAsync(fundDto.Id);
                fundDto.UserCreateName = employees.FirstOrDefault(x => x.Id == fundDto.UserCreateId)?.Name;
            }

            return fundDtos;
        }
    }
}
