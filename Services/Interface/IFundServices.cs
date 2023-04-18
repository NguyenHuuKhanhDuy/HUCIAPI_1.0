using ApplicationCore.ModelsDto.Fund;
using ApplicationCore.ViewModels.Fund;

namespace Services.Interface
{
    public interface IFundServices
    {
        Task<FundDto> CreateFundAsync(FundVM fundVM);
    }
}
