using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Commission;
using ApplicationCore.ViewModels.Commission;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class CommissionImp : BaseServices, ICommissionServices
    {
        private readonly HucidbContext _dbContext;
        public CommissionImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionVM"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<CommissionDto> CreateComissionAsync(CommissionVM commissionVM)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == commissionVM.UserCreateId && !x.IsDeleted);
            if (employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var commission = MapFCommissionVMTCommission(commissionVM);
            await _dbContext.Commissions.AddAsync(commission);
            await _dbContext.SaveChangesAsync();

            var commissionDto = MapFCommissionTCommissionDto(commission, employee.Name);
            return commissionDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteComissionByIdAsync(Guid commissionId)
        {
            var commission = await FindCommissionAsync(commissionId);
            commission.IsDelete = true;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Commission> FindCommissionAsync(Guid commissionId)
        {
            var commission = await _dbContext.Commissions.FirstOrDefaultAsync(x => x.Id == commissionId && !x.IsDelete);
            if(commission == null)
            {
                throw new BusinessException(CommissionConstants.CommissionNotExists);
            }

            return commission;
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<CommissionDto>> GetAllCommissionAsync()
        {
            var commissions = await _dbContext.Commissions.Where(x => !x.IsDelete)
                                    .OrderByDescending(x => x.TotalPriceFrom).ToListAsync();
            var employees = await _dbContext.Employees.ToListAsync();

            var commissionDtos = new List<CommissionDto>();

            foreach (var commission in commissions)
            {
                var employee = employees.FirstOrDefault(x => x.Id == commission.UserCreateId && !x.IsDeleted);
                commissionDtos.Add(MapFCommissionTCommissionDto(commission, employee?.Name));
            }

            return commissionDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommissionDto> GetCommissionByIdAsync(Guid commissionId)
        {
            var commission = await FindCommissionAsync(commissionId);
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == commission.UserCreateId);
            
            if(employee == null)
            {
                throw new BusinessException(CommissionConstants.CommissionNotExists);
            }

            var commissionDto = MapFCommissionTCommissionDto(commission, employee.Name);
            return commissionDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommissionDto> UpdateComissionAsync(CommissionUpdateVM commissionVM)
        {
            var commission = await FindCommissionAsync(commissionVM.Id);
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == commission.UserCreateId);

            if (employee == null)
            {
                throw new BusinessException(CommissionConstants.CommissionNotExists);
            }

            MapFCommissionUpdateVMTCommission(commissionVM, commission);
            await _dbContext.SaveChangesAsync();

            var commissionDto = MapFCommissionTCommissionDto(commission, employee.Name);

            return commissionDto;
        }
    }
}
