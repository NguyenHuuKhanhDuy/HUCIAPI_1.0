﻿using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Customer;
using ApplicationCore.ModelsDto.Fund;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ModelsDto.Promotion;
using ApplicationCore.ModelsDto.Supplier;
using ApplicationCore.ViewModels.Customer;
using ApplicationCore.ViewModels.Fund;
using ApplicationCore.ViewModels.Order;
using ApplicationCore.ViewModels.Product;
using ApplicationCore.ViewModels.Promotion;
using ApplicationCore.ViewModels.Supplier;
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

            if (location == null)
            {
                throw new BusinessException(BaseConstants.LOCATION_NOT_EXIST);
            }

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
            productDto.OnHand = product.OnHand;
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
            comboDto.OnHand = product.OnHand;
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
            foreach (Product product in products)
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

        //Map Supplier
        public void MapFSupplierVMTSupplier(Supplier supplier, SupplierVM supplierVM)
        {
            supplier.Name = supplierVM.Name;
            supplier.Email = supplierVM.Email;
            supplier.Phone = supplierVM.Phone;
            supplier.ProvinceId = supplierVM.ProvinceId;
            supplier.DistrictId = supplierVM.DistrictId;
            supplier.WardId = supplierVM.WardId;
            supplier.Notes = supplierVM.Notes;
            supplier.CreateUserId = supplierVM.CreateUserId;
            supplier.Address = supplierVM.Address;
        }

        public void MapFSupplierUpdateVMTSupplier(Supplier supplier, SupplierUpdateVM supplierVM)
        {
            supplier.Name = supplierVM.Name;
            supplier.Email = supplierVM.Email;
            supplier.Phone = supplierVM.Phone;
            supplier.ProvinceId = supplierVM.ProvinceId;
            supplier.DistrictId = supplierVM.DistrictId;
            supplier.WardId = supplierVM.WardId;
            supplier.Notes = supplierVM.Notes;
            supplier.Address = supplierVM.Address;
        }

        public SupplierDto MapFSupplierTSupplierDto(Supplier supplier)
        {
            SupplierDto dto = new SupplierDto();
            dto.Id = supplier.Id;
            dto.Name = supplier.Name;
            dto.Email = supplier.Email;
            dto.Phone = supplier.Phone;
            dto.ProvinceId = supplier.ProvinceId;
            dto.ProvinceName = supplier.ProvinceName;
            dto.DistrictId = supplier.DistrictId;
            dto.DistrictName = supplier.DistrictName;
            dto.WardId = supplier.WardId;
            dto.WardName = supplier.WardName;
            dto.Notes = supplier.Notes;
            dto.CreateUserId = supplier.CreateUserId;
            dto.CreateUserName = supplier.CreateUserName;
            dto.CreateDate = supplier.CreateDate;
            dto.Address = supplier.Address;

            return dto;
        }

        //Map Order
        public void MapFOrderVMTOrder(Order order, OrderVM orderVM)
        {
            order.CustomerId = orderVM.CustomerId;
            order.CustomerName = orderVM.CustomerName;
            order.CustomerPhone = orderVM.CustomerPhone;
            order.CustomerEmail = orderVM.CustomerEmail;
            order.CustomerAddress = orderVM.CustomerAddress;
            order.ProvinceId = orderVM.ProvinceId;
            order.DistrictId = orderVM.DistrictId;
            order.WardId = orderVM.WardId;
            order.VoucherId = orderVM.VoucherId;
            order.OrderDiscount = orderVM.OrderDiscount;
            order.OrderStatusId = orderVM.OrderStatusId;
            order.OrderStatusPaymentId = orderVM.OrderStatusPaymentId;
            order.OrderStatusShippingId = orderVM.OrderStatusShippingId;
            order.OrderShippingMethodId = orderVM.OrderShippingMethodId;
            order.OrderSourceId = orderVM.OrderSourceId;
            order.OrderNote = orderVM.OrderNote;
            order.CreateEmployeeId = orderVM.CreateEmployeeId;
        }

        public void MapFOrderUpdateVMTOrder(Order order, OrderUpdateVM orderVM)
        {
            order.VoucherId = orderVM.VoucherId;
            order.OrderStatusId = orderVM.OrderStatusId;
            order.OrderStatusPaymentId = orderVM.OrderStatusPaymentId;
            order.OrderStatusShippingId = orderVM.OrderStatusShippingId;
            order.OrderShippingMethodId = orderVM.OrderShippingMethodId;
            order.OrderSourceId = orderVM.OrderSourceId;
        }

        public OrderDto MapFOrderTOrderDto(Order order)
        {
            OrderDto orderDto = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                CustomerAddress = order.CustomerAddress,
                ProvinceId = order.ProvinceId,
                ProvinceName = order.ProvinceName,
                DistrictId = order.DistrictId,
                DistrictName = order.DistrictName,
                WardId = order.WardId,
                WardName = order.WardName,
                OrderTotal = order.OrderTotal,
                VoucherId = order.VoucherId,
                VoucherName = order.VoucherName,
                VoucherDiscount = order.VoucherDiscount,
                OrderDiscount = order.OrderDiscount,
                TotalOrderDiscount = order.TotalOrderDiscount,
                TotalPayment = order.TotalPayment,
                OrderStatusId = order.OrderStatusId,
                OrderStatusName = order.OrderStatusName,
                OrderStatusPaymentId = order.OrderStatusPaymentId,
                OrderStatusPaymentName = order.OrderStatusPaymentName,
                OrderStatusShippingId = order.OrderStatusShippingId,
                OrderStatusShippingName = order.OrderStatusShippingName,
                OrderShippingMethodId = order.OrderShippingMethodId,
                OrderShippingMethodName = order.OrderShippingMethodName,
                OrderNote = order.OrderNote,
                CreateEmployeeId = order.CreateEmployeeId,
                CreateEmployeeName = order.CreateEmployeeName,
                OrderSourceId = order.OrderSourceId,
                OrderSourceName = order.OrderSourceName,
            };

            return orderDto;
        }

        public OrderDetailDto MapFOrderDetailTOrderDetailDto(OrderDetail orderDetail)
        {
            return new OrderDetailDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                ProductNumber = orderDetail.ProductNumber,
                ProductName = orderDetail.ProductName,
                ProductImage = orderDetail.ProductImage,
                ProductPrice = orderDetail.ProductPrice,
                Discount = orderDetail.Discount,
                SubTotal = orderDetail.SubTotal,
                Quantity = orderDetail.Quantity
            };
        }

        //Map Promotion
        public Promotion MapFPromotionVMToPromotion(PromotionVM promotionVM)
        {
            return new Promotion
            {
                ProductId = promotionVM.ProductId,
                QuantityFrom = promotionVM.QuantityFrom,
                Price = promotionVM.Price,
                UserCreateId = promotionVM.UserCreateId
            };
        }

        public void MapFPromotionUpdateVMTPromotion(Promotion promotion, PromotionUpdateVM promotionVM)
        {
            promotion.ProductId = promotionVM.ProductId;
            promotion.Price = promotionVM.Price;
            promotion.QuantityFrom = promotionVM.QuantityFrom;
        }

        public PromotionDto MapFPromotionTPromotionDto(Promotion promotion)
        {
            return new PromotionDto
            {
                Id = promotion.Id,
                ProductId = promotion.ProductId,
                Price = promotion.Price,
                QuantityFrom = promotion.QuantityFrom,
                UserCreateId = promotion.UserCreateId,
                UserCreateName = promotion.UserCreateName,
                CreateDate = promotion.CreateDate
            };
        }

        //Map Fund
        public Fund MapFFundVMTFund(FundVM fundVM)
        {
            return new Fund
            {
                Id = Guid.NewGuid(),
                Name = fundVM.Name,
                TotalFund = fundVM.TotalFund,
                CreateDate = GetDateTimeNow(),
                IsActive = true,
                IsDeleted = false,
                UserCreateId = fundVM.UserCreateId,
                Note = fundVM.Note
            };
        }

        public FundDto MapFFundTFundDto(Fund fund)
        {
            return new FundDto
            {
                Id = fund.Id,
                Name = fund.Name,
                TotalFund = fund.TotalFund,
                CreateDate = fund.CreateDate,
                IsActive = fund.IsActive.HasValue ? fund.IsActive.Value : true,
                UserCreateId = fund.UserCreateId,
                Note = fund.Note
            };
        }

        public void MapFFundUpdateVMTFund(Fund fund, FundUpdateVM fundUpdateVM)
        {
            fund.Name = fundUpdateVM.Name;
            fund.TotalFund = fundUpdateVM.TotalFund;
            fund.Note = fundUpdateVM.Note;
        }

        //Fund Detail
        public FundDetail MapFFundDetailVMTFundDetail(FundDetailVM fundDetail)
        {
            return new FundDetail
            {
                Id = Guid.NewGuid(),
                FundId = fundDetail.FundId,
                AmountMoney = fundDetail.AmountMoney,
                CreateDate = GetDateTimeNow(),
                TypeFundId = fundDetail.TypeFundId,
                TypeFundName = fundDetail.TypeFundName,
                UserCreateId = fundDetail.UserCreateId,
                Note = fundDetail.Note
            };
        }

        public FundDetailDto MapFFundDetailTFundDetailDto(FundDetail fundDetail)
        {
            return new FundDetailDto
            {
                Id = fundDetail.Id,
                FundId = fundDetail.FundId,
                AmountMoney = fundDetail.AmountMoney,
                TypeFundId = fundDetail.TypeFundId,
                TypeFundName = fundDetail.TypeFundName,
                UserCreateId = fundDetail.UserCreateId,
                CreateDate = fundDetail.CreateDate,
                Note = fundDetail.Note
            };
        }
    }
}
