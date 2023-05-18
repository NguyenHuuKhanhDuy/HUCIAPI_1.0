using ApplicationCore.ModelsDto.IP;
using ApplicationCore.ViewModels.IP;

namespace Services.Interface
{
    public interface IIPServices
    {
        Task<IPDto> CreateIPAsync(IPVM vm);
        Task<IPDto> UpdateIPAsync(IPUpdateVM vm);
        Task DeleteIPAsync(Guid IPId);
        Task<List<IPDto>> GetAllIPsAsync();
    }
}
