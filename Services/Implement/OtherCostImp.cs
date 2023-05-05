using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.OtherCost;
using ApplicationCore.ViewModels.OtherCost;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System.Net.WebSockets;

namespace Services.Implement
{
    public class OtherCostImp : BaseServices, IOtherCostServices
    {
        private readonly HucidbContext _dbContext;
        
        public OtherCostImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OtherCostDto> CreateOtherCostAsync(OtherCostVM vm)
        {
            var userCreateName = await GetUserCreateNameAsync(vm.UserCreateId);

            var otherCost = MapFOtherCostVMTOtherCost(vm);

            await _dbContext.AddAsync(otherCost);
            await _dbContext.SaveChangesAsync();

            var otherCostDto = MapFOtherCostTOtherCostDto(otherCost, userCreateName);

            return otherCostDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private async Task<string> GetUserCreateNameAsync(Guid employeeId)
        {
            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == employeeId && !x.IsDeleted);

            if(employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            return employee.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherCostId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<OtherCost> CheckOtherCostId(Guid otherCostId)
        {
            var otherCost = await _dbContext.OtherCosts.FirstOrDefaultAsync(x => x.Id == otherCostId && !x.IsDeleted);

            if(otherCost == null)
            {
                throw new BusinessException(OtherCostConstants.OtherCostNotExist);
            }

            return otherCost;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherCostId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteOtherCostAsync(Guid otherCostId)
        {
            var otherCost = await CheckOtherCostId(otherCostId);

            otherCost.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<OtherCostDto>> GetAllOtherCostAsync()
        {
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var otherCosts = await _dbContext.OtherCosts.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
            var otherCostDtos = GetOtherCostDtosFromOtherCost(otherCosts, employees);

            return otherCostDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherCosts"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        private List<OtherCostDto> GetOtherCostDtosFromOtherCost(List<OtherCost> otherCosts, List<Employee> employees)
        {
            var otherCostDtos = new List<OtherCostDto>();

            foreach (var otherCost in otherCosts)
            {
                var employee = employees.FirstOrDefault(x => x.Id == otherCost.UserCreateId);

                if (employee == null)
                    continue;

                otherCostDtos.Add(MapFOtherCostTOtherCostDto(otherCost, employee.Name));
            }

            return otherCostDtos;
        }
        public async Task<OtherCostDto> UpdateOtherCostAsync(OtherCostUpdateVM vm)
        {
            var otherCost = await CheckOtherCostId(vm.Id);

            MapFOtherCostUpdateVMTOtherCost(otherCost, vm);

            await _dbContext.SaveChangesAsync();

            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == otherCost.UserCreateId);


            var dto = MapFOtherCostTOtherCostDto(otherCost, employee?.Name);

            return dto;
        }
    }
}
