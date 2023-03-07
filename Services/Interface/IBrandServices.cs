using ApplicationCore.ModelsDto.Brand;
using ApplicationCore.ViewModels.Brand;

namespace Services.Interface
{
    public interface IBrandServices
    {
        Task<BrandDto> CreateBrandAsync(BrandVM brandVM);

        Task<BrandDto> UpdateBrandAsync(BrandUpdateVM brandVM);

        Task DeleteBrandAsync(Guid brandId);

        Task<List<BrandDto>> GetAllBrandsAsync();
    }
}
