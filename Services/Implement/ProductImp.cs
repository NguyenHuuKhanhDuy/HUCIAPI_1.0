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
            product.IsDeleted = false;

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

            MapListFProductsTProductDtos(products, productDtos);

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

            MapListFProductsTProductDtos(products, productDtos);

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

            MapListFProductsTProductDtos(products, productDtos);

            return productDtos;
        }

        private async Task<List<ProductDto>> GetProductByIdsAsync(List<ProductInsideComboVM> productVMs)
        {
            var products = await _dbContext.Products.Where(x => !x.IsDeleted && productVMs.Select(y => y.ProductId).Contains(x.Id)).ToListAsync();

            if (products.Count == 0)
            {
                throw new BusinessException(ProductConstants.PRODUCTS_IN_COMBO_NOT_EXIST);
            }

            List<ProductDto> productDtos = new List<ProductDto>();

            foreach (var productVM in productVMs)
            {
                var product = products.Where(x => x.Id == productVM.ProductId).FirstOrDefault();

                if (product.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
                {
                    throw new BusinessException(ProductConstants.CANNOT_ADD_COMBO_INSIDE_COMBO);
                }
                else if (product == null)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_NOT_EXIST} : Id = {productVM.ProductId}");
                }
                else if (product.OnHand == 0)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_INSIDE_COMBO_QUANTITY_0} : Id = {productVM.ProductId}");
                }

                productDtos.Add(MapFProductTProductDto(product));
            }

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

        public async Task<ComboDto> CreateComboAsync(ComboVM comboVM)
        {
            ComboDto comboDto = new ComboDto();
            List<ComboDetail> comboDetails = new List<ComboDetail>();

            Product product = MapFComboVMTProduct(comboVM);
            product.Id = Guid.NewGuid();

            comboDto.products = await GetProductByIdsAsync(comboVM.products);

            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(comboVM.BrandId, comboVM.CategoryId, comboVM.ProductTypeId, comboVM.UserCreateId, product);
            product.ProductTypeName = nameRelationOfProduct.ProductTypeName;
            product.ProductNumber = GetNumberProduct();
            product.IsActive = true;
            product.CreateDate = GetDateTimeNow();
            product.IsDeleted = false;

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            foreach (var item in comboDto.products)
            {
                ComboDetail comboDetail = new ComboDetail();

                comboDetail.Id = Guid.NewGuid();
                comboDetail.ComboId = product.Id;
                comboDetail.ProductId = item.Id;
                comboDetail.IsDelete = false;
                comboDetail.Quantity = comboVM.products.Where(y => y.ProductId == comboDetail.ProductId).FirstOrDefault().Quantity;
                comboDetails.Add(comboDetail);
            }

            await _dbContext.ComboDetails.AddRangeAsync(comboDetails);
            await _dbContext.SaveChangesAsync();

            MapFProductTComboDto(product, comboDto);

            return comboDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboVM"></param>
        /// <returns></returns>
        public async Task<ComboDto> UpdateComboAsync(ComboUpdateVM comboVM)
        {
            Product product = await CheckExistProduct(comboVM.Id);
            MapFComboUpdateVMTProduct(comboVM, product);

            List<ProductDto> productDtos = await GetProductByIdsAsync(comboVM.products);

            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(comboVM.BrandId, comboVM.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            ComboDto comboDto = new ComboDto();
            MapFProductTComboDto(product, comboDto);

            List<ComboDetail> comboDetailUpdates = await _dbContext.ComboDetails.Where(x => x.ComboId == product.Id).ToListAsync();
            comboDto.products = productDtos;

            List<ComboDetail> comboDetailAdds = UpdateProductInsideCombo(productDtos, comboVM.products, comboDetailUpdates, product.Id);

            await _dbContext.ComboDetails.AddRangeAsync(comboDetailAdds);
            await _dbContext.SaveChangesAsync();

            return comboDto;
        }

        public List<ComboDetail> UpdateProductInsideCombo(List<ProductDto> productDtos, List<ProductInsideComboVM> productInsideComboVMs, List<ComboDetail> comboDetails, Guid comboId)
        {
            List<ComboDetail> comboDetails1 = new List<ComboDetail>();

            foreach (var product in productDtos)
            {
                var checkExist = comboDetails.Where(x => x.ProductId == product.Id).FirstOrDefault();
                if(checkExist == null)
                {
                    ComboDetail comboDetail = new ComboDetail();
                    comboDetail.Id = Guid.NewGuid();
                    comboDetail.ComboId = comboId;
                    comboDetail.ProductId = product.Id;
                    comboDetail.Quantity = productInsideComboVMs.Where(y => y.ProductId == product.Id).FirstOrDefault().Quantity;
                    comboDetails1.Add(comboDetail);
                }
            }

            foreach (var comboDetail in comboDetails)
            {
                var checkExist = productDtos.Where(x => x.Id == comboDetail.ProductId).FirstOrDefault();
                comboDetail.IsDelete = checkExist == null ? true : false;
                comboDetail.Quantity = productInsideComboVMs.Where(y => y.ProductId == comboDetail.ProductId).FirstOrDefault().Quantity;
            }

            return comboDetails1;
        }

    }
}
