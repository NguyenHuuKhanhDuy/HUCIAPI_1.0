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
    public class ProductImp : BaseServices, IProductServices
    {

        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBrandServices _brandServices;
        private readonly ICategoryServices _categoryServices;
        public ProductImp(HucidbContext dbContext, IBrandServices brandServices, ICategoryServices categoryServices)
        {
            _dbContext = dbContext;
            _brandServices = brandServices;
            _categoryServices = categoryServices;
        }

        public async Task<ProductDto> CreateProductAsync(ProductVM productVM)
        {
            Product product = MapFProductVMTProduct(productVM);
            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(productVM.BrandId, productVM.CategoryId, productVM.ProductTypeId, productVM.UserCreateId, product);

            product.Id = Guid.NewGuid();
            product.ProductTypeName = nameRelationOfProduct.ProductTypeName;
            product.ProductNumber = GetNumberProduct();
            product.IsActive = true;
            product.CreateDate = GetDateTimeNow();

            ProductDto productDto = MapFProductTProductDto(product);
            productDto.BrandName = nameRelationOfProduct.BrandName;
            productDto.CategoryName = nameRelationOfProduct.CategoryName;
            productDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            productDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return productDto;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productUpdateVM"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateProductAsync(ProductUpdateVM productUpdateVM)
        {
            Product product = await CheckExistProduct(productUpdateVM.Id);
            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(productUpdateVM.BrandId, productUpdateVM.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            MapFProductUpdateVMTProduct(productUpdateVM, product);

            await _dbContext.SaveChangesAsync();

            ProductDto productDto = MapFProductTProductDto(product);
            productDto.BrandName = nameRelationOfProduct.BrandName;
            productDto.CategoryName = nameRelationOfProduct.CategoryName;
            productDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            productDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;

            return productDto;
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            Product product = await CheckExistProduct(productId);

            product.IsDeleted = true;
            product.ProductNumber += BaseConstants.DELETE;
            product.Name += BaseConstants.DELETE;

            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Product> CheckExistProduct(Guid productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null || product.IsDeleted)
            {
                throw new BusinessException(ProductConstants.PRODUCT_NOT_EXIST);
            }

            return product;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="categoryId"></param>
        /// <param name="productTypeId"></param>
        /// <param name="employeeId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<NameRelationOfProduct> GetNameRelationOfProduct(Guid brandId, Guid categoryId, int productTypeId, Guid employeeId, Product product)
        {
            var brand = await _dbContext.Brands.Where(x => x.Id == brandId && !x.IsDeleted).FirstOrDefaultAsync();
            if (brand == null)
            {
                throw new BusinessException(BrandConstants.BRAND_NOT_EXIST);
            }
            product.Brand = brand;

            var category = await _dbContext.Categories.Where(x => x.Id == categoryId && !x.IsDeleted).FirstOrDefaultAsync();
            if (category == null)
            {
                throw new BusinessException(CategoryConstants.CATEGORY_NOT_EXIST);
            }
            product.Category = category;

            var user = await _dbContext.Employees.Where(x => x.Id == employeeId && !x.IsDeleted).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }
            product.UserCreate = user;

            var productType = await _dbContext.ProductTypes.FindAsync(productTypeId);
            if (productType == null)
            {
                throw new BusinessException(ProductConstants.PRODUCT_TYPE_NOT_EXIST);
            }
            product.ProductType = productType;

            NameRelationOfProduct result = new NameRelationOfProduct(brand.Name, category.Name, user.Name, productType.Name);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetNumberProduct()
        {
            int number = _dbContext.Products.Count() + 1;
            return ProductConstants.PREFIX_PRODUCT_NUMBER + number;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            List<ProductDto> productDtos = new List<ProductDto>();

            List<Product> products = await _dbContext.Products.Where(x => !x.IsDeleted).ToListAsync();

            MapListFProductVMTProduct(products, productDtos);

            return productDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _dbContext.Products.Where(x => x.Id == productId && !x.IsDeleted).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new BusinessException(ProductConstants.PRODUCT_NOT_EXIST);
            }

            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(product.BrandId, product.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            ProductDto productDto = MapFProductTProductDto(product);
            productDto.BrandName = nameRelationOfProduct.BrandName;
            productDto.CategoryName = nameRelationOfProduct.CategoryName;
            productDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            productDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;

            return productDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductByBrandIdAsync(Guid brandId)
        {
            var products = await _dbContext.Products.Where(x => x.BrandId == brandId && !x.IsDeleted).ToListAsync();
            List<ProductDto> productDtos = new List<ProductDto>();

            MapListFProductVMTProduct(products, productDtos);

            return productDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductByCategoryIdAsync(Guid categoryId)
        {
            var products = await _dbContext.Products.Where(x => x.CategoryId == categoryId && !x.IsDeleted).ToListAsync();
            List<ProductDto> productDtos = new List<ProductDto>();

            MapListFProductVMTProduct(products, productDtos);

            return productDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataForCreateProductDto> GetDataForCreateProductAsync()
        {
            DataForCreateProductDto dataForCreateProductDto = new DataForCreateProductDto();

            dataForCreateProductDto.Brands = await _brandServices.GetAllBrandsAsync();
            dataForCreateProductDto.Categorys = await _categoryServices.GetAllCategoriesAsync();

            List<ProductType> productTypes = await _dbContext.ProductTypes.ToListAsync();
            foreach (ProductType productType in productTypes)
            {
                ProductTypeDto productTypeDto = new ProductTypeDto();
                productTypeDto.Id = productType.Id;
                productTypeDto.Name = productType.Name;
                dataForCreateProductDto.ProductTypes.Add(productTypeDto);
            }

            return dataForCreateProductDto;
        }
    }
}
