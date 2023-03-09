using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Product;

namespace Services.Interface
{
    public interface IProductServices
    {
        Task<ProductDto> CreateProductAsync(ProductVM productVM);
    }
}
