using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Shift;
using ApplicationCore.ViewModels.Shift;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class ShiftImp : BaseServices, IShiftServices
    {
        private readonly HucidbContext _dbContext;
        public ShiftImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<ShiftDto> CreateShiftAsync(ShiftVM vm)
        {
            var shift = new Shift
            {
                Id = Guid.NewGuid(),
                EndTime = ParseStringToTimeSpan(vm.EndTime),
                StartTime = ParseStringToTimeSpan(vm.StartTime),
                CreateDate = GetDateTimeNow(),
                IsDeleted = BaseConstants.IsDeletedDefault
            };

            await _dbContext.AddAsync(shift);
            await _dbContext.SaveChangesAsync();

            var dto = MapFShiftTShiftDto(shift);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public async Task DeleteShiftAsync(Guid shiftId)
        {
            var shift = await FindShiftAsync(shiftId);

            shift.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShiftDto>> GetAllShiftAsync()
        {
            var shifts = await _dbContext.Shifts.Where(x => !x.IsDeleted).ToListAsync();
            var dtos = new List<ShiftDto>();

            foreach (var shift in shifts)
            {
                dtos.Add(MapFShiftTShiftDto(shift));
            }

            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<ShiftDto> UpdateShiftAsync(ShiftUpdateVM vm)
        {
            var shift = await FindShiftAsync(vm.Id);
            shift.StartTime = ParseStringToTimeSpan(vm.StartTime);
            shift.EndTime = ParseStringToTimeSpan(vm.EndTime);

            var dto = MapFShiftTShiftDto(shift);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<Shift> FindShiftAsync(Guid shiftId)
        {
            var shift = await _dbContext.Shifts.FirstOrDefaultAsync(x => x.Id == shiftId && !x.IsDeleted);

            if (shift == null)
            {
                throw new BusinessException(ShiftConstant.ShiftNotExist);
            }

            return shift;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private TimeSpan ParseStringToTimeSpan(string timeString)
        {
            TimeSpan timeSpan;
            bool success = TimeSpan.TryParse(timeString, out timeSpan);
            if (!success)
            {
                throw new BusinessException(ShiftConstant.CanNotParseToTime);
            }

            return timeSpan;
        }
    }
}
