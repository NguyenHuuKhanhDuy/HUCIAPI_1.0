using ApplicationCore.ModelsDto.OtherCost;
using ApplicationCore.ViewModels.OtherCost;

namespace Services.Interface
{
    public interface IOtherCostServices
    {
        Task<OtherCostDto> CreateOtherCostAsync(OtherCostVM vm);
        Task<OtherCostDto> UpdateOtherCostAsync(OtherCostUpdateVM vm);
        Task DeleteOtherCostAsync(Guid otherCostId);
        Task<List<OtherCostDto>> GetAllOtherCostAsync();
    }
}
