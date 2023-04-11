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
            Order order = new Order
            {
                CustomerId = Guid.Empty,
                ProvinceId = BaseConstants.INT_DEFAULT,
                DistrictId = BaseConstants.INT_DEFAULT,
                WardId = BaseConstants.INT_DEFAULT,
                VoucherId = Guid.Empty,
                OrderStatusId = BaseConstants.INT_DEFAULT,
                OrderStatusPaymentId = BaseConstants.INT_DEFAULT,
                OrderStatusShippingId = BaseConstants.INT_DEFAULT,
                OrderShippingMethodId = BaseConstants.INT_DEFAULT,
                OrderSourceId = BaseConstants.INT_DEFAULT
            };
            MapFOrderVMTOrder(order, orderVM);

            await CheckInforForOrder(order);

            order.Id = Guid.NewGuid();
            order.OrderNumber = await GetOrderNumber();
            order.OrderDate = GetDateTimeNow();

            var productDtos = await GetProductDtoByIdsAsync(orderVM.products);
            var orderDetails = await GetOrderDetailsAndCalculatePrice(order, productDtos, orderVM.products);

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
            await _dbContext.SaveChangesAsync();

            OrderDto dto = MapFOrderTOrderDto(order);
            dto.products = orderDetails;
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="productDtos"></param>
        /// <param name="discounts"></param>
        /// <returns></returns>
        private async Task<List<OrderDetail>> GetOrderDetailsAndCalculatePrice(Order order, List<ProductDto> productDtos, List<ProductInsideOrderVM> discounts)
        {
            int total = BaseConstants.INT_DEFAULT;
            int voucherDiscount = BaseConstants.INT_DEFAULT;
            int productDiscount = BaseConstants.INT_DEFAULT;
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
                        SubTotal = (product.Price - discount.Discount) * discount.Quantity,
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
                else if (voucher.DiscountPercent != 0)
                {
                    voucherDiscount = (int)(((double)voucher.DiscountPercent / 100.0) * (total - productDiscount));

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrderNumber()
        {
            int number = await _dbContext.Orders.CountAsync() + 1;
            return OrderConstants.PREFIX_ORDER_NUMBER + number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task CheckInforForOrder(Order order)
        {
            var voucher = await _dbContext.Vouchers.FirstOrDefaultAsync(x => x.Id == order.VoucherId);

            if (voucher == null)
            {
                throw new BusinessException(OrderConstants.VOUCHER_NOT_EXISTS);
            }

            if (voucher.Quantity == 0)
            {
                throw new BusinessException(OrderConstants.VOUCHER_EXCEED);
            }
            order.VoucherName = voucher.Name;

            var status = await _dbContext.StatusOrders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.OrderStatusId);

            if (status == null)
            {
                throw new BusinessException(OrderConstants.ORDER_STATUS_NOT_EXISTS);
            }

            order.OrderStatusName = status.Name;

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == order.CustomerId);

            if (customer == null)
            {
                throw new BusinessException(OrderConstants.CUSTOMER_NOT_EXISTS);
            }

            order.CustomerName = customer.Name;
            order.CustomerPhone = customer.Phone;
            order.CustomerEmail = customer.Email;
            order.CustomerAddress = customer.Address;
            customer.OrderCount += 1;

            var paymentStatus = await _dbContext.StatusPayments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.OrderStatusPaymentId);

            if (paymentStatus == null)
            {
                throw new BusinessException(OrderConstants.PAYMENT_STATUS_NOT_EXISTS);
            }

            order.OrderStatusPaymentName = paymentStatus.Name;

            var statusShipping = await _dbContext.StatusShippings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.OrderStatusShippingId);

            if (paymentStatus == null)
            {
                throw new BusinessException(OrderConstants.SHIPPING_STATUS_NOT_EXISTS);
            }

            order.OrderStatusShippingName = statusShipping.Name;

            var shippingMethod = await _dbContext.ShippingMethods.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.OrderShippingMethodId);

            if (shippingMethod == null)
            {
                throw new BusinessException(OrderConstants.SHIPPING_METHOD_NOT_EXISTS);
            }

            order.OrderShippingMethodName = shippingMethod.Name;

            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.CreateEmployeeId);

            if (employee == null)
            {
                throw new BusinessException(OrderConstants.USER_CREATE_NOT_EXISTS);
            }

            order.CreateEmployeeName = employee.Name;

            var source = await _dbContext.OrderSources.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.OrderSourceId);

            if (employee == null)
            {
                throw new BusinessException(OrderConstants.SOURCE_ORDER_NOT_EXISTS);
            }

            order.OrderSourceName = source.SourceName;
            order.ProvinceName = await GetNameLocationById(order.ProvinceId);
            order.DistrictName = await GetNameLocationById(order.DistrictId);
            order.WardName = await GetNameLocationById(order.WardId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productVMs"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<List<ProductDto>> GetProductDtoByIdsAsync(List<ProductInsideOrderVM> productVMs)
        {
            var products = await _dbContext.Products.Where(x => !x.IsDeleted && productVMs
                                                    .Select(x => x.ProductId)
                                                    .Contains(x.Id))
                                                    .ToListAsync();

            if (products.Count == 0)
            {
                throw new BusinessException(ProductConstants.PRODUCTS_IN_COMBO_NOT_EXIST);
            }

            List<ProductDto> productDtos = new List<ProductDto>();

            foreach (var productVM in productVMs)
            {
                var product = products.Where(x => x.Id == productVM.ProductId).FirstOrDefault();

                if (product == null)
                {
                    throw new BusinessException($"{ProductConstants.PRODUCT_NOT_EXIST} : Id = {productVM}");
                }

                productDtos.Add(MapFProductTProductDto(product));
                product.OnHand = product.OnHand - productVM.Quantity;
            }

            return productDtos.OrderBy(x => x.ProductNumber).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && !x.IsDeleted);

            if (order == null)
            {
                throw new BusinessException(OrderConstants.ORDER_NOT_EXISTS);
            }

            await HandleCancelOrDeleteOrderAsync(order);
            order.IsDeleted = true;
            order.OrderNumber += BaseConstants.DELETE;
            await _dbContext.SaveChangesAsync();
        }

        public async Task HandleCancelOrDeleteOrderAsync(Order order)
        {
            var orderDetails = await _dbContext.OrderDetails.AsNoTracking().Where(x => x.OrderId == order.Id).ToListAsync();
            var products = await _dbContext.Products
                                           .Where(x => !x.IsDeleted && orderDetails.Select(x => x.ProductId).ToList().Contains(x.Id))
                                           .ToListAsync();

            if (products.Any())
            {
                foreach (var product in products)
                {
                    var orderDetail = orderDetails.FirstOrDefault(x => x.ProductId == product.Id);

                    if (orderDetail != null)
                    {
                        product.OnHand += orderDetail.Quantity;
                    }
                }
            }

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == order.CustomerId && !x.IsDeleted);

            if (customer != null)
            {
                customer.OrderCount -= 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDto> UpdateOrderAsync(OrderUpdateVM orderVM)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderVM.Id && !x.IsDeleted);

            if (order == null)
            {
                throw new BusinessException(OrderConstants.ORDER_NOT_EXISTS);
            }

            MapFOrderUpdateVMTOrder(order, orderVM);
            await CheckInforForOrder(order);

            await _dbContext.SaveChangesAsync();

            var dto = MapFOrderTOrderDto(order);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetAllOrderAsync()
        {
            List<OrderDto> orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => !x.IsDeleted).ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    orderDtos.Add(MapFOrderTOrderDto(order));
                }
            }
            
            return orderDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetOrderByDate(DateTime startDate, DateTime endDate)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date).ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    orderDtos.Add(MapFOrderTOrderDto(order));
                }
            }

            return orderDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetOrderByStatusId(int statusId)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => x.OrderStatusId == statusId).ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    orderDtos.Add(MapFOrderTOrderDto(order));
                }
            }

            return orderDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrderDto> GetDetailOrderById(Guid orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id== orderId);

            if(order == null)
            {
                throw new Exception(OrderConstants.ORDER_NOT_EXISTS);
            }

            var orderDetails = await _dbContext.OrderDetails.Where(x => x.OrderId == order.Id).ToListAsync();
            var orderDto = MapFOrderTOrderDto(order);
            
            if(orderDetails.Any())
            {
                orderDto.products = orderDetails.Select(x => MapFOrderDetailTOrderDetailDto(x)).ToList();
            }

            return orderDto;
        }
    }
}
