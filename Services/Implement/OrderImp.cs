using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Order;
using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class OrderImp : BaseServices, IOrderServices
    {
        private readonly HucidbContext _dbContext;
        public OrderImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDto> CreateOrderAsync(OrderVM orderVM)
        {
            NameRelationOfOrder nameRelationOfOrder = await CheckInforForOrder(orderVM);

            Order order = new Order();
            MapFOrderVMTOrder(order, orderVM);
            order.Id = Guid.NewGuid();
            order.OrderNumber = await GetOrderNumber();
            order.OrderDate = GetDateTimeNow();
            order.ProvinceName = nameRelationOfOrder.Province;
            order.DistrictName = nameRelationOfOrder.District;
            order.WardName = nameRelationOfOrder.Ward;
            order.VoucherName = nameRelationOfOrder.Voucher;
            order.OrderStatusName = nameRelationOfOrder.OrderStatus;
            order.OrderStatusPaymentName = nameRelationOfOrder.PaymentStatus;
            order.OrderStatusShippingName = nameRelationOfOrder.ShippingStatus;
            order.OrderShippingMethodName = nameRelationOfOrder.ShippingMethod;
            order.CreateEmployeeName = nameRelationOfOrder.Employee;
            order.OrderSourceName = nameRelationOfOrder.Source;
            
            List<ProductDto> productDtos = await GetProductDtoByIdsAsync(orderVM.products.Select(x => x.ProductId).ToList());
            List<OrderDetail> orderDetails = await GetOrderDetailsAndCalculatePrice(order, productDtos, orderVM.products);

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
            await _dbContext.SaveChangesAsync();

            OrderDto dto = MapFOrderTOrderDto(order);
            dto.products = orderDetails;
            return dto;
        }

        private async Task<List<OrderDetail>> GetOrderDetailsAndCalculatePrice(Order order, List<ProductDto> productDtos, List<ProductInsideOrderVM> discounts)
        {
            int total = 0;
            int voucherDiscount = 0;
            int productDiscount = 0;
            List<OrderDetail> orderDetails = new List<OrderDetail>();


            foreach (ProductInsideOrderVM discount in discounts)
            {
                var product = productDtos.FirstOrDefault(x => x.Id == discount.ProductId);
                if (product != null)
                {
                    productDiscount += discount.Discount * discount.Quantity;
                    total += product.Price * discount.Quantity;

                    orderDetails.Add(new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = product.Id,
                        ProductNumber = product.ProductNumber,
                        ProductName = product.Name,
                        ProductImage = product.Image,
                        ProductPrice = product.Price,
                        Discount = discount.Discount,
                        SubTotal = product.Price - discount.Discount,
                        Quantity = discount.Quantity
                    });
                }
            }

            order.OrderTotal = total;
            var voucher = await _dbContext.Vouchers.FirstOrDefaultAsync(x => x.Id == order.VoucherId && x.Id != Guid.Empty);

            if (voucher != null)
            {
                if (voucher.DiscountPrice != 0)
                {
                    voucherDiscount = voucher.DiscountPrice;

                    if (voucherDiscount > total)
                    {
                        order.TotalPayment = 0;
                    }
                }
                else if(voucher.DiscountPercent != 0)
                {
                    voucherDiscount = (int)(((double) voucher.DiscountPercent / 100.0) * (total - productDiscount));

                    if (voucherDiscount > total)
                    {
                        order.TotalPayment = 0;
                    }
                }

                order.TotalOrderDiscount = voucherDiscount + productDiscount + order.OrderDiscount;

                return orderDetails;
            }

            order.VoucherDiscount = voucherDiscount;
            order.TotalOrderDiscount = voucherDiscount + productDiscount + order.OrderDiscount;
            order.TotalPayment = total - order.TotalOrderDiscount;

            return orderDetails;
        }
        public async Task<string> GetOrderNumber()
        {
            int number = await _dbContext.Orders.CountAsync() + 1;
            return OrderConstants.PREFIX_ORDER_NUMBER + number;
        }

        private async Task<NameRelationOfOrder> CheckInforForOrder(OrderVM orderVM)
        {
            NameRelationOfOrder nameRelationOfOrder = new NameRelationOfOrder();
            var voucher = await _dbContext.Vouchers.FirstOrDefaultAsync(x => x.Id == orderVM.VoucherId);

            if (voucher == null)
            {
                throw new BusinessException(OrderConstants.VOUCHER_NOT_EXISTS);
            }

            if (voucher.Quantity == 0)
            {
                throw new BusinessException(OrderConstants.VOUCHER_EXCEED);
            }
            nameRelationOfOrder.Voucher = voucher.Name;

            var status = await _dbContext.StatusOrders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.OrderStatusId);

            if (status == null)
            {
                throw new BusinessException(OrderConstants.ORDER_STATUS_NOT_EXISTS);
            }

            nameRelationOfOrder.OrderStatus = status.Name;

            var paymentStatus = await _dbContext.StatusPayments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.OrderStatusPaymentId);

            if (paymentStatus == null)
            {
                throw new BusinessException(OrderConstants.PAYMENT_STATUS_NOT_EXISTS);
            }

            var statusShipping = await _dbContext.StatusShippings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.OrderStatusShippingId);

            if (paymentStatus == null)
            {
                throw new BusinessException(OrderConstants.SHIPPING_STATUS_NOT_EXISTS);
            }

            nameRelationOfOrder.ShippingStatus = statusShipping.Name;

            var shippingMethod = await _dbContext.ShippingMethods.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.OrderShippingMethodId);

            if (shippingMethod == null)
            {
                throw new BusinessException(OrderConstants.SHIPPING_METHOD_NOT_EXISTS);
            }

            nameRelationOfOrder.ShippingMethod = shippingMethod.Name;

            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.CreateEmployeeId);

            if (employee == null)
            {
                throw new BusinessException(OrderConstants.USER_CREATE_NOT_EXISTS);
            }

            nameRelationOfOrder.Employee = employee.Name;

            var source = await _dbContext.OrderSources.AsNoTracking().FirstOrDefaultAsync(x => x.Id == orderVM.OrderSourceId);

            if (employee == null)
            {
                throw new BusinessException(OrderConstants.SOURCE_ORDER_NOT_EXISTS);
            }

            nameRelationOfOrder.Source = source.SourceName;
            nameRelationOfOrder.Province = await GetNameLocationById(orderVM.ProvinceId);
            nameRelationOfOrder.District = await GetNameLocationById(orderVM.DistrictId);
            nameRelationOfOrder.Ward = await GetNameLocationById(orderVM.WardId);
            return nameRelationOfOrder;
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

                if (product == null)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_NOT_EXIST} : Id = {productVM}");
                }

                productDtos.Add(MapFProductTProductDto(product));
            }

            return productDtos.OrderBy(x => x.ProductNumber).ToList();
        }


        public Task DeleteOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> UpdateOrderAsync(OrderVM orderVM)
        {
            throw new NotImplementedException();
        }
    }
}
