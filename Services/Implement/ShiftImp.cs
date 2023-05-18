using ApplicationCore.ModelsDto.Shift;
using ApplicationCore.ViewModels.Shift;
using Infrastructure.Models;
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

        public Task<ShiftDto> CreateShiftAsync(ShiftVM vm)
        {
            throw new NotImplementedException();
        }

        public Task DeleteShiftAsync(Guid shiftId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShiftDto>> GetAllShiftAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShiftDto> UpdateShiftAsync(ShiftUpdateVM vm)
        {
            throw new NotImplementedException();
        }
    }
}
