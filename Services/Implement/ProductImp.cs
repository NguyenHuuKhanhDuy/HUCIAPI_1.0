using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Interface;

namespace Services.Implement
{
    public class ProductImp : BaseServices, IProductServices
    {

        private readonly HucidbContext _dbContext;
        private readonly IBrandServices _brandServices;
        private readonly ICategoryServices _categoryServices;
        public ProductImp(HucidbContext dbContext, IBrandServices brandServices, ICategoryServices categoryServices) : base(dbContext)
        {
            _dbContext = dbContext;
            _brandServices = brandServices;
            _categoryServices = categoryServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productVM"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateProductAsync(ProductVM productVM)
        {
            var product = DataMapper.Map<ProductVM, Product>(productVM);
            product.ProductTypeId = ProductConstants.PRODUCT_TYPE_PRODUCT;

            var nameRelationOfProduct = await GetNameRelationOfProduct(productVM.BrandId, productVM.CategoryId, productVM.ProductTypeId, productVM.UserCreateId, product);

            product.ProductTypeName = nameRelationOfProduct.ProductTypeName;
            product.Id = Guid.NewGuid();
            product.ProductNumber = GetNumberProduct(product.ProductTypeId);
            product.IsActive = true;
            product.CreateDate = GetDateTimeNow();
            product.IsDeleted = false;

            var productDto = DataMapper.Map<Product, ProductDto>(product);
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
            var product = await CheckExistProduct(productUpdateVM.Id);
            var nameRelationOfProduct = await GetNameRelationOfProduct(productUpdateVM.BrandId, productUpdateVM.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            MapFProductUpdateVMTProduct(productUpdateVM, product);

            await _dbContext.SaveChangesAsync();

            var productDto = DataMapper.Map<Product, ProductDto>(product);
            productDto.BrandName = nameRelationOfProduct.BrandName;
            productDto.CategoryName = nameRelationOfProduct.CategoryName;
            productDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            productDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;

            return productDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await CheckExistProduct(productId);

            product.IsDeleted = true;
            product.ProductNumber += BaseConstants.DELETE;
            product.Name += BaseConstants.DELETE;

            if (product.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
            {
                var comboDetails = await _dbContext.ComboDetails.Where(x => x.ComboId == productId).ToListAsync();

                foreach (var checkDetail in comboDetails)
                {
                    checkDetail.IsDelete = true;
                }
            }

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
        public string GetNumberProduct(int productTypeId)
        {
            int number = _dbContext.Products.Count() + 1;
            return productTypeId == 0 ? (ProductConstants.PREFIX_PRODUCT_NUMBER + number)
                                      : (ProductConstants.PREFIX_COMBO_NUMBER + number);
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _dbContext.Products
                                 .Include(x => x.Brand)
                                 .Include(x => x.Category)
                                 .Include(x => x.UserCreate)
                                 .Where(x => !x.IsDeleted).ToListAsync();

            var productDtos = GetDetailProductAsync(products);

            return productDtos.OrderByDescending(x => x.CreateDate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ComboDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _dbContext.Products.Where(x => x.Id == productId && !x.IsDeleted).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new BusinessException(ProductConstants.PRODUCT_NOT_EXIST);
            }

            var nameRelationOfProduct = await GetNameRelationOfProduct(product.BrandId, product.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            var comboDto = new ComboDto();
            MapFProductTComboDto(product, comboDto);
            comboDto.BrandName = nameRelationOfProduct.BrandName;
            comboDto.CategoryName = nameRelationOfProduct.CategoryName;
            comboDto.UserCreateName = nameRelationOfProduct.UserCreateName;
            comboDto.ProductTypeName = nameRelationOfProduct.ProductTypeName;

            if (comboDto.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
            {
                int min = int.MaxValue;
                int? temp = 0;
                var comboDetails = await _dbContext.ComboDetails.Where(x => x.ComboId == product.Id).ToListAsync();
                var productInsideComboIds = comboDetails.Select(x => x.ProductId).ToList();
                comboDto.products = await GetProductDtoByIdsAsync(productInsideComboIds);

                foreach (var pd in comboDto.products)
                {
                    temp = pd.OnHand / comboDetails.FirstOrDefault(x => x.ProductId == pd.Id)?.Quantity;
                    if (min > temp)
                    {
                        min = temp.Value;
                    }

                    pd.OnHand = comboDetails.FirstOrDefault(x => x.ProductId == pd.Id).Quantity;
                }

                comboDto.OnHand = min;
            }

            return comboDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductByBrandIdAsync(Guid brandId)
        {
            var products = await _dbContext.Products
                                .Include(x => x.Brand)
                                .Include(x => x.Category)
                                .Include(x => x.UserCreate)
                                .Where(x => x.BrandId == brandId && !x.IsDeleted).ToListAsync();

            var productDtos =  GetDetailProductAsync(products);

            return productDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductByCategoryIdAsync(Guid categoryId)
        {
            var products = await _dbContext.Products
                                 .Include(x => x.Brand)
                                 .Include(x => x.Category)
                                 .Include(x => x.UserCreate)
                                 .Where(x => x.CategoryId == categoryId && !x.IsDeleted).ToListAsync();
            var productDtos = GetDetailProductAsync(products);

            return productDtos;
        }

        private async Task<List<ProductDto>> GetProductDtoByIdsAsync(List<Guid> productVMs)
        {
            var products = await _dbContext.Products.Where(x => !x.IsDeleted && productVMs.Contains(x.Id)).ToListAsync();

            if (products.Count == 0)
            {
                throw new BusinessException(ProductConstants.PRODUCTS_IN_COMBO_NOT_EXIST);
            }

            List<ProductDto> productDtos = new List<ProductDto>();

            foreach (var productVM in productVMs)
            {
                var product = products.Where(x => x.Id == productVM).FirstOrDefault();

                if (product.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
                {
                    throw new BusinessException(ProductConstants.CANNOT_ADD_COMBO_INSIDE_COMBO);
                }
                else if (product == null)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_NOT_EXIST} : Id = {productVM}");
                }
                else if (product.OnHand == 0)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_INSIDE_COMBO_QUANTITY_0} : Id = {productVM}");
                }

                productDtos.Add(DataMapper.Map<Product, ProductDto>(product));
            }

            return productDtos.OrderBy(x => x.ProductNumber).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<DataForCreateProductDto> GetDataForCreateProductAsync()
        {
            var dataForCreateProductDto = new DataForCreateProductDto();

            dataForCreateProductDto.Brands = await _brandServices.GetAllBrandsAsync();
            dataForCreateProductDto.Categorys = await _categoryServices.GetAllCategoriesAsync();

            var productTypes = await _dbContext.ProductTypes.ToListAsync();
            foreach (var productType in productTypes)
            {
                var productTypeDto = new ProductTypeDto();
                productTypeDto.Id = productType.Id;
                productTypeDto.Name = productType.Name;
                dataForCreateProductDto.ProductTypes.Add(productTypeDto);
            }

            return dataForCreateProductDto;
        }

        public async Task<ComboDto> CreateComboAsync(ComboVM comboVM)
        {
            var comboDto = new ComboDto();
            var comboDetails = new List<ComboDetail>();

            var product = DataMapper.Map<ComboVM, Product>(comboVM);

            product.Id = Guid.NewGuid();
            product.ProductTypeId = ProductConstants.PRODUCT_TYPE_COMBO;

            comboDto.products = await GetProductDtoByIdsAsync(comboVM.products.Select(x => x.ProductId).ToList());

            NameRelationOfProduct nameRelationOfProduct = await GetNameRelationOfProduct(comboVM.BrandId, comboVM.CategoryId, comboVM.ProductTypeId, comboVM.UserCreateId, product);
            product.ProductTypeName = nameRelationOfProduct.ProductTypeName;
            product.ProductNumber = GetNumberProduct(product.ProductTypeId);
            product.IsActive = true;
            product.CreateDate = GetDateTimeNow();
            product.IsDeleted = false;

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            foreach (var item in comboDto.products)
            {
                var comboDetail = new ComboDetail();

                comboDetail.Id = Guid.NewGuid();
                comboDetail.ComboId = product.Id;
                comboDetail.ProductId = item.Id;
                comboDetail.IsDelete = false;
                comboDetail.Quantity = comboVM.products.Where(y => y.ProductId == comboDetail.ProductId).FirstOrDefault().Quantity;
                comboDetails.Add(comboDetail);
            }

            await _dbContext.ComboDetails.AddRangeAsync(comboDetails);
            await _dbContext.SaveChangesAsync();

            comboDto = DataMapper.Map<Product, ComboDto>(product);

            return comboDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comboVM"></param>
        /// <returns></returns>
        public async Task<ComboDto> UpdateComboAsync(ComboUpdateVM comboVM)
        {
            var product = await CheckExistProduct(comboVM.Id);
            MapFComboUpdateVMTProduct(comboVM, product);

            var productDtos = await GetProductDtoByIdsAsync(comboVM.products.Select(x => x.ProductId).ToList());

            var nameRelationOfProduct = await GetNameRelationOfProduct(comboVM.BrandId, comboVM.CategoryId, product.ProductTypeId, product.UserCreateId, product);

            var comboDto = DataMapper.Map<Product, ComboDto>(product);

            var comboDetailUpdates = await _dbContext.ComboDetails.Where(x => x.ComboId == product.Id).ToListAsync();
            comboDto.products = productDtos;

            var comboDetailAdds = UpdateProductInsideCombo(productDtos, comboVM.products, comboDetailUpdates, product.Id);

            await _dbContext.ComboDetails.AddRangeAsync(comboDetailAdds);
            await _dbContext.SaveChangesAsync();

            return comboDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDtos"></param>
        /// <param name="productInsideComboVMs"></param>
        /// <param name="comboDetails"></param>
        /// <param name="comboId"></param>
        /// <returns></returns>
        public List<ComboDetail> UpdateProductInsideCombo(List<ProductDto> productDtos, List<ProductInsideComboVM> productInsideComboVMs, List<ComboDetail> comboDetails, Guid comboId)
        {
            var comboDetails1 = new List<ComboDetail>();

            foreach (var product in productDtos)
            {
                var checkExist = comboDetails.Where(x => x.ProductId == product.Id).FirstOrDefault();
                if (checkExist == null)
                {
                    var comboDetail = new ComboDetail();
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
                if (checkExist != null)
                {
                    comboDetail.IsDelete = false;
                    comboDetail.Quantity = (int)productInsideComboVMs.Where(y => y.ProductId == comboDetail.ProductId).FirstOrDefault()?.Quantity;
                }
                else
                {
                    comboDetail.IsDelete = true;
                }
            }

            return comboDetails1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public List<ProductDto> GetDetailProductAsync(List<Product> products)
        {
            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = DataMapper.Map<Product, ProductDto>(product);
                productDto.BrandName = product.Brand.Name;
                productDto.CategoryName = product.Category.Name;
                productDto.UserCreateName = product.UserCreate.Name;
                productDtos.Add(productDto);
            }

            return productDtos;
        }
    }
}
