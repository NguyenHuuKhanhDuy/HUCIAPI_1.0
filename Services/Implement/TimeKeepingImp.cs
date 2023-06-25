using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels.TimeKeeping;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class TimeKeepingImp : BaseServices, ITimeKeepingServices
    {
        private readonly HucidbContext _dbContext;
        public TimeKeepingImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<TimeKeepingDto> CreateTimeKeepingAsync(TimeKeepingVM vm)
        {
            var userCreate = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vm.UserCreateId && !x.IsDeleted);

            if(userCreate == null)
            {
                throw new BusinessException(TimeKeepingConstants.UserCreateNotExist);
            }

            var userTimeKeeping = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vm.UserTimeKeepingId && !x.IsDeleted);

            if (userTimeKeeping == null)
            {
                throw new BusinessException(TimeKeepingConstants.TimeKeepingNotExist);
            }

            var timeKeeping = new TimeKeeping
            {
                Id = Guid.NewGuid(),
                UserCreateId = vm.UserCreateId,
                UserTimeKeepingId = vm.UserTimeKeepingId,
                IsDeleted = BaseConstants.IsDeletedDefault,
                CreateDate = GetDateTimeNow()
            };

            await _dbContext.TimeKeepings.AddAsync(timeKeeping);
            await _dbContext.SaveChangesAsync();

            var timeKeepingDto = MapFTimeKeepingTTimeKeepingDto(timeKeeping, userCreate.Name, userTimeKeeping.Name);

            return timeKeepingDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeKeepingId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task DeleteTimeKeepingAsync(Guid timeKeepingId)
        {
            var timeKeeping = await _dbContext.TimeKeepings.FirstOrDefaultAsync(x => x.Id == timeKeepingId && !x.IsDeleted);

            if(timeKeeping == null)
            {
                throw new BusinessException(TimeKeepingConstants.TimeKeepingNotExist);
            }

            timeKeeping.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TimeKeepingDto>> GetAllTimeKeepingAsync()
        {
            DateTime currentDate = DateTime.Now; // You can use any DateTime object here

            // Get the first day of the month
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            // Get the last day of the month by getting the first day of the next month and subtracting one day
            DateTime lastDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1).AddDays(-1);

            var timeKeepings = await _dbContext.TimeKeepings.AsNoTracking().Where(x => x.CreateDate.Date <= lastDayOfMonth.Date 
                                                                    && x.CreateDate >= firstDayOfMonth.Date 
                                                                    && !x.IsDeleted)
                                                                    .OrderByDescending(x => x.CreateDate)
                                                                    .ToListAsync();
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var timeKeepingDtos = new List<TimeKeepingDto>();

            foreach (var item in timeKeepings)
            {
                var userCreate = employees.FirstOrDefault(x => x.Id == item.UserCreateId);
                var userTimeKeeping = employees.FirstOrDefault(x => x.Id == item.UserTimeKeepingId);

                timeKeepingDtos.Add(MapFTimeKeepingTTimeKeepingDto(item, userCreate.Name, userTimeKeeping.Name));
            }

            return timeKeepingDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TimeKeepingDto>> GetAllTimeKeepingByDateAsync(DateTime startDate, DateTime endDate)
        {
            var timeKeepings = await _dbContext.TimeKeepings.AsNoTracking()
                .Where(x => !x.IsDeleted
                    && x.CreateDate.Date >= startDate.Date
                    && x.CreateDate.Date <= endDate.Date)
                .ToListAsync();

            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var timeKeepingDtos = new List<TimeKeepingDto>();

            foreach (var item in timeKeepings)
            {
                var userCreate = employees.FirstOrDefault(x => x.Id == item.UserCreateId);
                var userTimeKeeping = employees.FirstOrDefault(x => x.Id == item.UserTimeKeepingId);

                timeKeepingDtos.Add(MapFTimeKeepingTTimeKeepingDto(item, userCreate.Name, userTimeKeeping.Name));
            }

            return timeKeepingDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<TimeKeepingDto>> GetAllTimeKeepingByEmployeeIdAsync(Guid employeeId)
        {
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == employeeId && !x.IsDeleted);

            if(employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var userCreateName = employee.Name;
            var timeKeepings = await _dbContext.TimeKeepings.AsNoTracking()
                .Where(x => !x.IsDeleted && x.UserTimeKeepingId == employeeId)
                .ToListAsync();

            var timeKeepingDtos = new List<TimeKeepingDto>();

            foreach (var item in timeKeepings)
            {
                var userTimeKeeping = employees.FirstOrDefault(x => x.Id == item.UserTimeKeepingId);

                timeKeepingDtos.Add(MapFTimeKeepingTTimeKeepingDto(item, userCreateName, userTimeKeeping.Name));
            }

            return timeKeepingDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TimeKeepingDto>> GetAllTimeKeepingByEmployeeIdAndDateAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();
            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == employeeId && !x.IsDeleted);

            if (employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var userCreateName = employee.Name;
            var timeKeepings = await _dbContext.TimeKeepings.AsNoTracking()
                .Where(x => !x.IsDeleted 
                    && x.UserTimeKeepingId == employeeId
                    && x.CreateDate.Date >= startDate.Date
                    && x.CreateDate.Date <= endDate.Date)
                .ToListAsync();

            var timeKeepingDtos = new List<TimeKeepingDto>();

            foreach (var item in timeKeepings)
            {
                var userTimeKeeping = employees.FirstOrDefault(x => x.Id == item.UserTimeKeepingId);

                timeKeepingDtos.Add(MapFTimeKeepingTTimeKeepingDto(item, userCreateName, userTimeKeeping.Name));
            }

            return timeKeepingDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<bool> IsTimeKeepingTodayAsync(Guid employeeId)
        {
            var timeKeeping = await _dbContext.TimeKeepings.AsNoTracking().FirstOrDefaultAsync(x => x.UserTimeKeepingId == employeeId && !x.IsDeleted && x.CreateDate.Date == GetDateTimeNow().Date);

            if (timeKeeping != null)
                return true;

            return false;
        }
    }
}
