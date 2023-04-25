using ApplicationCore.ModelsDto.Commission;
using ApplicationCore.ViewModels.Commission;

namespace Services.Interface
{
    public interface ICommissionServices
    {
        Task<CommissionDto> CreateComissionAsync(CommissionVM commissionVM);
        Task<CommissionDto> UpdateComissionAsync(CommissionUpdateVM commissionVM);
        Task DeleteComissionByIdAsync(Guid commissionId);
        Task<List<CommissionDto>> GetAllCommissionAsync();
        Task<CommissionDto> GetCommissionByIdAsync(Guid commissionId);
    }
}
