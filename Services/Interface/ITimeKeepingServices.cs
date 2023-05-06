using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels.TimeKeeping;

namespace Services.Interface
{
    public interface ITimeKeepingServices
    {
        Task<TimeKeepingDto> CreateTimeKeepingAsync(TimeKeepingVM vm);
        Task DeleteTimeKeepingAsync(Guid timeKeepingId);
        Task<bool> IsTimeKeepingTodayAsync(Guid employeeId);
        Task<List<TimeKeepingDto>> GetAllTimeKeepingByEmployeeIdAsync(Guid employeeId);
        Task<List<TimeKeepingDto>> GetAllTimeKeepingByEmployeeIdAndDateAsync(Guid employeeId, DateTime startDate, DateTime endDate);
        Task<List<TimeKeepingDto>> GetAllTimeKeepingAsync();
        Task<List<TimeKeepingDto>> GetAllTimeKeepingByDateAsync(DateTime startDate, DateTime endDate);
    }
}
