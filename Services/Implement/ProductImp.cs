using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Product;
using AutoMapper;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class ProductImp : IProductServices
    {

        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductImp(HucidbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductDto> CreateProductAsync(ProductVM productVM)
        {
            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(productVM);
            Product product = _mapper.Map<Product>(productVM);

            product.Id = Guid.NewGuid();

            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            ProductDto productDto = _mapper.Map<ProductDto>(product);
            productDto.BrandName = nameRelationOfProduct.BrandName;
            productDto.CategoryName = nameRelationOfProduct.CategoryName;
            productDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            productDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;
            return productDto;
        }

        private async Task<NameRelationOfProduct> GetNameRelationOfProduct(ProductVM pd)
        {
            var brand = await _dbContext.Brands.Where(x => x.Id == pd.BrandId && !x.IsDeleted).FirstOrDefaultAsync();
            if (brand == null)
            {
                throw new BusinessException(BrandConstants.BRAND_NOT_EXIST);
            }

            var category = await _dbContext.Categories.Where(x => x.Id == pd.CategoryId && !x.IsDeleted).FirstOrDefaultAsync();
            if (category == null)
            {
                throw new BusinessException(CategoryConstants.CATEGORY_NOT_EXIST);
            }

            var user = await _dbContext.Employees.Where(x => x.Id == pd.UserCreateId && !x.IsDeleted).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var productType = await _dbContext.ProductTypes.FindAsync(pd.ProductTypeId);
            if (productType == null)
            {
                throw new BusinessException(ProductConstants.PRODUCT_TYPE_NOT_EXIST);
            }

            NameRelationOfProduct result = new NameRelationOfProduct(brand.Name, category.Name, user.Name, productType.Name);
            return result;
        }

        public string GetNumberProduct()
        {
            int number = _dbContext.Products.Count() + 1;
            return ProductConstants.PREFIX_PRODUCT_NUMBER + number;
        }
    }
}
