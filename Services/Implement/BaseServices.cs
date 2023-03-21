using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Customer;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Customer;
using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Implement
{
    public abstract class BaseServices
    {
        private readonly HucidbContext _dbContext;
        public BaseServices(HucidbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CheckUserCreate(Guid? userCreateId)
        {
            var userCreate = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userCreateId && !x.IsDeleted);
            if (userCreate == null)
            {
                throw new BusinessException(BaseConstants.USER_CREATE_NOT_EXIST);
            }
        }

        /// <summary>
        /// Get Location By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>name of location</returns>
        public async Task<string> GetNameLocationById(int id)
        {
            var location = await _dbContext.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return location.Name;
        }

        public string FormatDateTime(DateTime dt, string format)
        {
            return dt.ToString(format);
        }

        public DateTime GetDateTimeNow()
        {
            //time Vietnam
            return DateTime.UtcNow.AddHours(7);
        }

        //Map product
        public ProductDto MapFProductTProductDto(Product product)
        {
            ProductDto productDto = new ProductDto();
            productDto.Id = product.Id;
            productDto.ProductNumber = product.ProductNumber;
            productDto.Name = product.Name;
            productDto.Price = product.Price;
            productDto.WholesalePrice = product.WholesalePrice;
            productDto.Image = product.Image;
            productDto.Quantity = product.OnHand;
            productDto.IsActive = product.IsActive.Value;
            productDto.BrandId = product.BrandId;
            productDto.CategoryId = product.CategoryId;
            productDto.Description = product.Description;
            productDto.CreateDate = product.CreateDate;
            productDto.ProductTypeId = product.ProductTypeId;
            productDto.ProductTypeName = product.ProductTypeName;
            productDto.UserCreateId = product.UserCreateId;

            return productDto;
        }

        public void MapFProductTComboDto(Product product, ComboDto comboDto)
        {
            comboDto.Id = product.Id;
            comboDto.ProductNumber = product.ProductNumber;
            comboDto.Name = product.Name;
            comboDto.Price = product.Price;
            comboDto.WholesalePrice = product.WholesalePrice;
            comboDto.Image = product.Image;
            comboDto.Quantity = product.OnHand;
            comboDto.IsActive = product.IsActive.Value;
            comboDto.BrandId = product.BrandId;
            comboDto.CategoryId = product.CategoryId;
            comboDto.Description = product.Description;
            comboDto.CreateDate = product.CreateDate;
            comboDto.ProductTypeId = product.ProductTypeId;
            comboDto.ProductTypeName = product.ProductTypeName;
            comboDto.UserCreateId = product.UserCreateId;
        }

        public Product MapFProductVMTProduct(ProductVM productVM)
        {
            Product product = new Product();
            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.WholesalePrice = productVM.WholesalePrice;
            product.Image = productVM.Image;
            product.OnHand = productVM.OnHand;
            product.BrandId = productVM.BrandId;
            product.CategoryId = productVM.CategoryId;
            product.Description = productVM.Description;
            product.ProductTypeId = productVM.ProductTypeId;
            product.UserCreateId = productVM.UserCreateId;

            return product;
        }

        public Product MapFComboVMTProduct(ComboVM productVM)
        {
            Product product = new Product();
            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.WholesalePrice = productVM.WholesalePrice;
            product.Image = productVM.Image;
            product.OnHand = 0;
            product.BrandId = productVM.BrandId;
            product.CategoryId = productVM.CategoryId;
            product.Description = productVM.Description;
            product.ProductTypeId = productVM.ProductTypeId;
            product.UserCreateId = productVM.UserCreateId;

            return product;
        }

        public void MapFComboUpdateVMTProduct(ComboUpdateVM productVM, Product product)
        {
            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.WholesalePrice = productVM.WholesalePrice;
            product.Image = productVM.Image;
            product.OnHand = 0;
            product.BrandId = productVM.BrandId;
            product.CategoryId = productVM.CategoryId;
            product.Description = productVM.Description;
        }

        public void MapFProductUpdateVMTProduct(ProductUpdateVM productUpdateVM, Product product)
        {
            product.Name = productUpdateVM.Name;
            product.Price = productUpdateVM.Price;
            product.WholesalePrice = productUpdateVM.WholesalePrice;
            product.OnHand = productUpdateVM.Quantity;
            product.BrandId = productUpdateVM.BrandId;
            product.CategoryId = productUpdateVM.CategoryId;
            product.Description = productUpdateVM.Description;
        }

        public void MapListFProductsTProductDtos(List<Product> products, List<ProductDto> productDtos)
        {
            foreach(Product product in products)
            {
                productDtos.Add(MapFProductTProductDto(product));
            }
        }

        //Map customer
        public void MapFCustomerVMTCustomer(Customer customer, CustomerVM customerVM)
        {
            customer.Name = customerVM.Name;
            customer.Email = customerVM.Email;
            customer.Phone = customerVM.Phone;
            customer.Birthday = customerVM.Birthday;
            customer.ProvinceId = customerVM.ProvinceId;
            customer.DistrictId = customerVM.DistrictId;
            customer.WardId = customerVM.WardId;
            customer.Notes = customerVM.Notes;
            customer.CreateUserId = customerVM.CreateUserId;
            customer.IpV4 = customerVM.IpV4;
            customer.Address = customerVM.Address;
        }

        public void MapFCustomerUpdateVMTCustomer(Customer customer, CustomerUpdateVM customerVM)
        {
            customer.Name = customerVM.Name;
            customer.Email = customerVM.Email;
            customer.Phone = customerVM.Phone;
            customer.Birthday = customerVM.Birthday;
            customer.ProvinceId = customerVM.ProvinceId;
            customer.DistrictId = customerVM.DistrictId;
            customer.WardId = customerVM.WardId;
            customer.Notes = customerVM.Notes;
            customer.IpV4 = customerVM.IpV4;
            customer.Address = customerVM.Address;
        }

        public CustomerDto MapFCustomerTCustomerDto(Customer customer)
        {
            CustomerDto dto = new CustomerDto();
            dto.Id = customer.Id;
            dto.Name = customer.Name;
            dto.Email = customer.Email;
            dto.Phone = customer.Phone;
            dto.Birthday = customer.Birthday;
            dto.Gender = customer.Gender;
            dto.ProvinceId = customer.ProvinceId;
            dto.ProvinceName = customer.ProvinceName;
            dto.DistrictId = customer.DistrictId;
            dto.DistrictName = customer.DistrictName;
            dto.WardId = customer.WardId;
            dto.WardName = customer.WardName;
            dto.Notes = customer.Notes;
            dto.OrderCount = customer.OrderCount;
            dto.CreateUserId = customer.CreateUserId;
            dto.CreateUserName = customer.CreateUserName;
            dto.IpV4 = customer.IpV4;
            dto.CreateDate = customer.CreateDate;
            dto.Address = customer.Address;

            return dto;
        }
    }
}
