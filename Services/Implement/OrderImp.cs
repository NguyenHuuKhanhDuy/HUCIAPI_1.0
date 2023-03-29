using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Order;
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
            return null;
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

            nameRelationOfOrder.Voucher = voucher.Name;

            var status = await _dbContext.StatusOrders.FirstOrDefaultAsync(x => x.Id == orderVM.OrderStatusId);

            if (status == null)
            {
                throw new BusinessException(OrderConstants.ORDER_STATUS_NOT_EXISTS);
            }

            nameRelationOfOrder.OrderStatus = status.Name;

            var paymentStatus = await _dbContext.StatusPayments.FirstOrDefaultAsync(x => x.Id == orderVM.OrderStatusPaymentId);

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
