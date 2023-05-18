using ApplicationCore.ModelsDto.Shift;
using ApplicationCore.ViewModels.Shift;

namespace Services.Interface
{
    public interface IShiftServices
    {
        Task<ShiftDto> CreateShiftAsync(ShiftVM vm);
        Task<ShiftDto> UpdateShiftAsync(ShiftUpdateVM vm);
        Task DeleteShiftAsync(Guid shiftId);
        Task<List<ShiftDto>> GetAllShiftAsync();
    }
}
