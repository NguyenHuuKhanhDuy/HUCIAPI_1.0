using ApplicationCore.ModelsDto.Fund;
using ApplicationCore.ViewModels.Fund;

namespace Services.Interface
{
    public interface IFundServices
    {
        Task<FundDto> CreateFundAsync(FundVM fundVM);
        Task<FundDto> UpdateFundAsync(FundUpdateVM fundVM);
        Task DeleteFundAsync(Guid fundId);
    }
}
