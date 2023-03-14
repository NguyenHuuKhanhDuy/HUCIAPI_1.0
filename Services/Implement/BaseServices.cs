using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Product;
using Infrastructure.Models;

namespace Services.Implement
{
    public abstract class BaseServices
    {
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
            productDto.Quantity = product.Quantity;
            productDto.IsActive = product.IsActive.Value;
            productDto.BrandId = product.BrandId;
            productDto.CategoryId = product.CategoryId;
            product.Description = product.Description;
            product.CreateDate = product.CreateDate;
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
            comboDto.Quantity = product.Quantity;
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
            product.Quantity = productVM.Quantity;
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
            product.Quantity = productVM.Quantity;
            product.BrandId = productVM.BrandId;
            product.CategoryId = productVM.CategoryId;
            product.Description = productVM.Description;
            product.ProductTypeId = productVM.ProductTypeId;
            product.UserCreateId = productVM.UserCreateId;

            return product;
        }

        public void MapFProductUpdateVMTProduct(ProductUpdateVM productUpdateVM, Product product)
        {
            product.Name = productUpdateVM.Name;
            product.Price = productUpdateVM.Price;
            product.WholesalePrice = productUpdateVM.WholesalePrice;
            product.Quantity = productUpdateVM.Quantity;
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

    }
}
