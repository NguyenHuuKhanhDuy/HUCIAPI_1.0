using ApplicationCore.ModelsDto.Promotion;
using ApplicationCore.ViewModels.Promotion;

namespace Services.Interface
{
    public interface IPromotionServices
    {
        Task<PromotionDto> CreatePromotionAsync(PromotionVM promotionVM);
        Task<PromotionDto> UpdatePromotionAsync(PromotionUpdateVM promotionVM);
        Task DeletePromotionAsync(Guid promotionId);
        Task<List<PromotionDto>> GetAllPromotionAsync();
        Task<PromotionDto> GetPromotionByIdAsync(Guid promotionId);
    }
}
