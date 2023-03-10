using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Product;

namespace Services.Interface
{
    public interface IProductServices
    {
        Task<ProductDto> CreateProductAsync(ProductVM productVM);

        Task<ProductDto> UpdateProductAsync(ProductUpdateVM productUpdateVM);

        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> GetProductByIdAsync(Guid productId);

        Task<List<ProductDto>> GetProductByBrandIdAsync(Guid brandId);

        Task<List<ProductDto>> GetProductByCategoryIdAsync(Guid categoryId);

        Task<DataForCreateProductDto> GetDataForCreateProductAsync();

        Task DeleteProductAsync(Guid productId);
    }
}
