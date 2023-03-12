using ApplicationCore.ModelsDto.Brand;
using ApplicationCore.ModelsDto.Category;

namespace ApplicationCore.ModelsDto.Product
{
    public class DataForCreateProductDto
    {
        public List<BrandDto> Brands { get; set; } = new List<BrandDto>();
        
        public List<CategoryDto> Categorys { get; set; } = new List<CategoryDto>();

        public List<ProductTypeDto> ProductTypes { get; set; } = new List<ProductTypeDto>();
    }
}
