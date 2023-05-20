using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Customer;
using ApplicationCore.ViewModels.Order;
using ApplicationCore.ViewModels.Product;
using AutoMapper.Execution;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Services.Interface;
using System.Drawing.Printing;

namespace Services.Implement
{
    public class OrderImp : BaseServices, IOrderServices
    {
        private readonly HucidbContext _dbContext;
        private readonly ICustomerServices _customerServices;
        public OrderImp(HucidbContext dbContext, ICustomerServices customerServices) : base(dbContext)
        {
            _dbContext = dbContext;
            _customerServices = customerServices;
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
                OrderStatusId = 1,
                OrderStatusPaymentId = 1,
                OrderStatusShippingId = 1,
                OrderShippingMethodId = 1,
                OrderSourceId = BaseConstants.INT_DEFAULT
            };

            MapFOrderVMTOrder(order, orderVM);

            await CheckInforForOrder(order, orderVM);

            order.Id = Guid.NewGuid();
            order.OrderNumber = await GetOrderNumber();
            order.OrderDate = GetDateTimeNow();
            order.IsDeleted = order.IsRemovedCallTakeCare = BaseConstants.IsDeletedDefault;

            var productDtos = await GetProductDtoByIdsAsync(orderVM.products);
            var orderDetails = await GetOrderDetailsAndCalculatePrice(order, productDtos, orderVM.products);

            order.BenefitOrder = await CalculateBenefitOrder(order.OrderTotal, orderVM.products);
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
            await _dbContext.SaveChangesAsync();

            OrderDto dto = MapFOrderTOrderDto(order);

            if (orderDetails.Any())
            {
                dto.products = orderDetails.Select(x => MapFOrderDetailTOrderDetailDto(x)).ToList();
            }

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
        private async Task CheckInforForOrder(Order order, OrderVM orderVM)
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

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Phone == order.CustomerPhone || x.Id == order.CustomerId);

            if (customer == null)
            {
                var customerVM = new CustomerVM
                {
                    Name = orderVM.CustomerName,
                    Phone = orderVM.CustomerPhone,
                    Email = orderVM.CustomerEmail,
                    ProvinceId = BaseConstants.INT_DEFAULT,
                    DistrictId = BaseConstants.INT_DEFAULT,
                    WardId = BaseConstants.INT_DEFAULT,
                    Address = orderVM.CustomerAddress,
                    CreateUserId = orderVM.CreateEmployeeId,
                    Notes = ""
                };

                var customerDto = await _customerServices.CreateCustomerAsync(customerVM);
                var customerTemp = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Phone == order.CustomerPhone);

                customerTemp.OrderCount += 1;
                order.CustomerId = customerTemp.Id;
            }
            else
            {
                customer.OrderCount += 1;
                order.CustomerId = customer.Id;
            }

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

            if (source == null)
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
        /// <param name="orderVM"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task CheckInforForOrderForUpdate(Order order)
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

            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Phone == order.CustomerPhone);

            if (customer == null)
            {
                throw new BusinessException(CustomerConstants.CUSTOMER_NOT_EXIST);
            }

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

            if (source == null)
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

                if (product.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
                {
                    var comboDetails = await _dbContext.ComboDetails.Where(x => x.ComboId == product.Id && !x.IsDelete).ToListAsync(); ;

                    foreach (var item in comboDetails)
                    {
                        var productInsideCombo = products.Where(x => x.Id == item.ProductId && !x.IsDeleted).FirstOrDefault();
                        if (productInsideCombo != null)
                        {
                            productInsideCombo.OnHand = productInsideCombo.OnHand - item.Quantity;
                        }
                    }
                }
                else
                {
                    product.OnHand = product.OnHand - productVM.Quantity;
                }
            }

            return productDtos.OrderBy(x => x.ProductNumber).ToList();
        }

        private async Task<int> CalculateBenefitOrder(int orderTotal, List<ProductInsideOrderVM> productVMs)
        {
            var products = await _dbContext.Products.Where(x => !x.IsDeleted && productVMs
                                                    .Select(x => x.ProductId)
                                                    .Contains(x.Id))
                                                    .ToListAsync();

            var totalOriginPrice = products.Sum(x => x.OriginalPrice);
            var benefit = orderTotal - totalOriginPrice;

            return benefit;
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
            var orderDetails = await _dbContext.OrderDetails.AsNoTracking().Where(x => x.OrderId == order.Id && !x.IsDeleted).ToListAsync();
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
            await CheckInforForOrderForUpdate(order);

            await UpdateOrderDetailsForOrder(orderVM, order);
            if (order.OrderStatusId == OrderConstants.OrderStatusSuccess)
            {
                var commissions = await _dbContext.Commissions.Where(x => !x.IsDelete).ToListAsync();
                var orderCommissions = await _dbContext.OrderCommissions.ToListAsync();
                await CreateOrderCommission(order, commissions, orderCommissions);
            }

            await _dbContext.SaveChangesAsync();

            var dto = MapFOrderTOrderDto(order);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task UpdateOrderDetailsForOrder(OrderUpdateVM orderVM, Order order)
        {
            var orderDetailsDB = await _dbContext.OrderDetails.Where(x => x.OrderId == orderVM.Id).ToListAsync();

            var productIds = orderVM.products.Select(x => x.ProductId).ToList();
            var orderDetailsUpdate = orderDetailsDB.Where(x => productIds.Contains(x.ProductId)).ToList();

            var orderDetailsDelete = orderDetailsDB.Where(x => !productIds.Contains(x.ProductId)).ToList();

            var productInsideOrders = await GetProductDtoByIdsAsync(orderVM.products);
            var orderDetails = await GetOrderDetailsAndCalculatePrice(order, productInsideOrders, orderVM.products);

            var orderDetailAddDB = orderDetails.Where(x => !orderDetailsUpdate.Select(y => y.ProductId).Contains(x.ProductId)).ToList();

            foreach (var update in orderDetailsUpdate)
            {
                var productInside = orderVM.products.Where(x => x.ProductId == update.ProductId).FirstOrDefault();

                if (productInside == null)
                    continue;

                update.Quantity = productInside.Quantity;
                update.Discount = productInside.Discount;
            }

            foreach (var delete in orderDetailsDelete)
            {
                delete.IsDeleted = true;
            }

            await _dbContext.OrderDetails.AddRangeAsync(orderDetailAddDB);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetAllOrderAsync()
        {
            List<OrderDto> orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => !x.IsDeleted).OrderByDescending(x => x.OrderDate).ToListAsync();
            var ordersDetails = await _dbContext.OrderDetails.ToListAsync();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    var orderDto = MapFOrderTOrderDto(order);
                    var orderDetailForOrder = ordersDetails?.Where(x => x.OrderId == order.Id).ToList();

                    if (orderDetailForOrder != null && orderDetailForOrder.Any())
                    {
                        orderDto.products = orderDetailForOrder?.Select(x => MapFOrderDetailTOrderDetailDto(x)).ToList();
                    }

                    orderDtos.Add(orderDto);
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
        public async Task<List<OrderDto>> GetOrderByDateAsync(DateTime startDate, DateTime endDate)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date).OrderBy(x => x.OrderDate).ToListAsync();

            if (orders.Any())
            {
                orderDtos = await GetOrderWithOrderDetail(orders);
            }

            return orderDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetOrderByStatusIdAsync(int statusId)
        {
            var orderDtos = new List<OrderDto>();
            var orders = await _dbContext.Orders.Where(x => x.OrderStatusId == statusId).OrderBy(x => x.OrderDate).ToListAsync();

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
        public async Task<OrderDto> GetDetailOrderByIdAsync(Guid orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

            if (order == null)
            {
                throw new Exception(OrderConstants.ORDER_NOT_EXISTS);
            }

            var orderDetails = await _dbContext.OrderDetails.Where(x => x.OrderId == order.Id && !x.IsDeleted).ToListAsync();
            var orderDto = MapFOrderTOrderDto(order);

            if (orderDetails.Any())
            {
                orderDto.products = orderDetails?.Select(x => MapFOrderDetailTOrderDetailDto(x)).ToList();
            }

            return orderDto;
        }

        /// <summary>
        /// Create Order From Ladipage
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        public async Task<OrderDto> CreateOrderFromLadipageAsync(OrderForLadipageVM orderVM)
        {
            var order = await GetInformationForOrderLadipageAsync(orderVM);
            var orderDto = await CreateOrderAsync(order);

            return orderDto;
        }

        private async Task<OrderVM> GetInformationForOrderLadipageAsync(OrderForLadipageVM orderVM)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Phone == orderVM.Phone && !x.IsDeleted);
            var order = new OrderVM
            {
                CustomerName = orderVM.Name,
                CustomerPhone = orderVM.Phone,
                CustomerAddress = $"{orderVM.Address}, {orderVM.Ward}, {orderVM.District}, {orderVM.Province}",
                ProvinceId = BaseConstants.INT_DEFAULT,
                DistrictId = BaseConstants.INT_DEFAULT,
                WardId = BaseConstants.INT_DEFAULT,
                VoucherId = Guid.Empty,
                CreateEmployeeId = Guid.Empty,
                OrderDiscount = BaseConstants.INT_DEFAULT,
                OrderStatusId = 1,
                OrderSourceId = OrderConstants.ORDER_SOURCE_TIKTOK,
                OrderStatusPaymentId = 1,
                OrderStatusShippingId = 1,
                OrderShippingMethodId = BaseConstants.INT_DEFAULT,
                OrderNote = "Order from Ladipage"
            };

            if (customer == null)
            {
                var customerVM = new CustomerVM
                {
                    Name = orderVM.Name,
                    Phone = orderVM.Phone,
                    Email = string.Empty,
                    ProvinceId = BaseConstants.INT_DEFAULT,
                    DistrictId = BaseConstants.INT_DEFAULT,
                    WardId = BaseConstants.INT_DEFAULT,
                    Address = $"{orderVM.Address}, {orderVM.Ward}, {orderVM.District}, {orderVM.Province}",
                    CreateUserId = Guid.Parse(BaseConstants.ADMIN_ID),
                    Notes = "Customer create from Ladipage"
                };

                var customerDto = await _customerServices.CreateCustomerAsync(customerVM);
                order.CustomerId = customerDto.Id;
            }
            else
            {
                order.CustomerId = customer.Id;
            }

            //var productNumbersWithQuantitys = GetProductCodesFromName(orderVM.Product);
            //var products = await _dbContext.Products.Where(x => !x.IsDeleted).ToListAsync();

            //foreach (var productNumberWithQuantity in productNumbersWithQuantitys)
            //{
            //    string[] parts = productNumberWithQuantity.Split('_');

            //    // Extract the two parts
            //    string quantityString = parts[0];
            //    int quantity;
            //    bool success = int.TryParse(quantityString, out quantity);
            //    string productNumber = parts[1];

            //    var product = products.FirstOrDefault(x => x.ProductNumber == productNumber);
            //    if (product == null)
            //    {
            //        throw new BusinessException(ProductConstants.PRODUCT_NOT_EXIST);
            //    }



            //    var productInsideOrder = new ProductInsideOrderVM
            //    {
            //        ProductId = product.Id,
            //        Quantity = success ? quantity : 1,
            //        Discount = 0
            //    };

            //    order.products.Add(productInsideOrder);
            //}

            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => !x.IsDeleted && x.Name == orderVM.Product);

            var productInsideOrder = new ProductInsideOrderVM
            {
                ProductId = product.Id,
                Quantity = 1,
                Discount = 0
            };

            order.products.Add(productInsideOrder);

            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        private List<string> GetProductCodesFromName(string productName)
        {

            // Extract the comma-separated list of strings inside the *...* markers
            string[] spStrings = productName.Split('*')[1].Split(',');

            // Trim any leading or trailing spaces from each string in the array
            for (int i = 0; i < spStrings.Length; i++)
            {
                spStrings[i] = spStrings[i].Trim();
            }

            // Convert the array of strings into a list
            List<string> spList = new List<string>(spStrings);

            return spList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        public async Task<string> UpdateStatusShippingGHTKAsync(IFormFile excelFile)
        {
            CheckExtensionExcelFile(excelFile);
            if (excelFile == null || excelFile.Length == 0)
                throw new BusinessException("Excel file not found");

            string result = string.Empty;
            int orderNumberCol = 0;
            int statusOrderCol = 0;
            int shippingNoteCol = 0;
            int startIndex = 999999999;
            var orders = await _dbContext.Orders.ToListAsync();
            var statusOrders = await _dbContext.StatusOrders.ToListAsync();
            var commissions = await _dbContext.Commissions.Where(x => !x.IsDelete).ToListAsync();
            var orderCommissions = await _dbContext.OrderCommissions.ToListAsync();

            int statusId = statusOrders.Count + 1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;

                    // Iterate through rows and columns
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var cellValueCol1 = worksheet.Cells[row, 1].Value?.ToString().Trim();

                        if (cellValueCol1 == FileConstants.Stt)
                        {
                            for (int col = 1; col <= columnCount; col++)
                            {
                                var cellValue = worksheet.Cells[row, col].Value?.ToString().Trim();
                                if (cellValue == FileConstants.OrderNumberGHTK) orderNumberCol = col;
                                if (cellValue == FileConstants.StatusOrderGHTK) statusOrderCol = col;
                                if (cellValue == FileConstants.ShippingNoteGHTK) shippingNoteCol = col;
                            }

                            startIndex = row;
                        }

                        if (row > startIndex)
                        {
                            var orderNumberText = worksheet.Cells[row, orderNumberCol].Value?.ToString().Trim();
                            var order = orders.FirstOrDefault(x => x.OrderNumber == orderNumberText);

                            if (order != null)
                            {
                                var shippingNoteText = worksheet.Cells[row, shippingNoteCol].Value?.ToString().Trim();
                                var statusOrderText = worksheet.Cells[row, statusOrderCol].Value?.ToString().Trim();

                                if (string.IsNullOrEmpty(statusOrderText))
                                    continue;

                                var statusOrder = statusOrders.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name)
                                && RemoveUnicode(x.Name) == RemoveUnicode(statusOrderText));

                                if (statusOrder == null)
                                {
                                    statusOrder = new StatusOrder
                                    {
                                        Id = statusId,
                                        Name = statusOrderText
                                    };

                                    statusId++;
                                    await _dbContext.StatusOrders.AddAsync(statusOrder);
                                    await _dbContext.SaveChangesAsync();
                                }

                                order.OrderStatusId = statusOrder.Id;
                                order.OrderStatusName = statusOrder.Name;
                                order.OrderNote = shippingNoteText ?? string.Empty;

                                if (RemoveUnicode(statusOrderText) == FileConstants.Paid)
                                {
                                    //add Commission for order
                                    await CreateOrderCommission(order, commissions, orderCommissions);

                                }
                            }
                        }
                    }

                    if (orderNumberCol == 0 || statusOrderCol == 0 || statusOrderCol == 0)
                    {
                        throw new BusinessException("file wrong format");
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
        public async Task<string> UpdateStatusShippingEMSAsync(IFormFile excelFile)
        {
            CheckExtensionExcelFile(excelFile);
            if (excelFile == null || excelFile.Length == 0)
                throw new BusinessException("Excel file not found");

            string result = string.Empty;
            int orderNumberCol = 0;
            int statusOrderCol = 0;
            int shippingNoteCol = 0;
            int startIndex = 999999999;
            var orders = await _dbContext.Orders.ToListAsync();
            var statusOrders = await _dbContext.StatusOrders.ToListAsync();
            var commissions = await _dbContext.Commissions.Where(x => !x.IsDelete).ToListAsync();
            var orderCommissions = await _dbContext.OrderCommissions.ToListAsync();

            int statusId = statusOrders.Count + 1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var columnCount = worksheet.Dimension.Columns;

                    // Iterate through rows and columns
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var cellValueCol1 = worksheet.Cells[row, 1].Value?.ToString().Trim();

                        if (cellValueCol1 == FileConstants.TinhNhan)
                        {
                            for (int col = 1; col <= columnCount; col++)
                            {
                                var cellValue = worksheet.Cells[row, col].Value?.ToString().Trim();
                                if (cellValue == FileConstants.OrderNumberEMS) orderNumberCol = col;
                                if (cellValue == FileConstants.StatusOrderEMS) statusOrderCol = col;
                                if (cellValue == FileConstants.ShippingNoteEMS) shippingNoteCol = col;
                            }

                            startIndex = row;
                        }

                        if (row > startIndex)
                        {
                            var orderNumberText = worksheet.Cells[row, orderNumberCol].Value?.ToString().Trim();
                            var order = orders.FirstOrDefault(x => x.OrderNumber == orderNumberText);

                            if (order != null)
                            {
                                var shippingNoteText = worksheet.Cells[row, shippingNoteCol].Value?.ToString().Trim();
                                var statusOrderText = worksheet.Cells[row, statusOrderCol].Value?.ToString().Trim();

                                if (string.IsNullOrEmpty(statusOrderText))
                                    continue;

                                var statusOrder = statusOrders.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name)
                                && RemoveUnicode(x.Name) == RemoveUnicode(statusOrderText));

                                if (statusOrder == null)
                                {
                                    statusOrder = new StatusOrder
                                    {
                                        Id = statusId,
                                        Name = statusOrderText
                                    };

                                    statusId++;
                                    await _dbContext.StatusOrders.AddAsync(statusOrder);
                                    await _dbContext.SaveChangesAsync();
                                }

                                order.OrderStatusId = statusOrder.Id;
                                order.OrderStatusName = statusOrder.Name;
                                order.OrderNote = shippingNoteText ?? string.Empty;

                                if (RemoveUnicode(statusOrderText) == FileConstants.EMSSuccess)
                                {
                                    //add Commission for order
                                    await CreateOrderCommission(order, commissions, orderCommissions);

                                }
                            }
                        }
                    }

                    if (orderNumberCol == 0 || statusOrderCol == 0 || statusOrderCol == 0)
                    {
                        throw new BusinessException("file wrong format");
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelFile"></param>
        /// <exception cref="BusinessException"></exception>
        private void CheckExtensionExcelFile(IFormFile excelFile)
        {
            string extension = Path.GetExtension(excelFile.FileName);

            if (extension != FileConstants.ExtensionExcel1 && extension != FileConstants.ExtensionExcel2)
            {
                throw new BusinessException(FileConstants.FileNotSupport);
            }
        }

        /// <summary>
        /// Create order commission when status order is paid
        /// </summary>
        /// <param name="order"></param>
        /// <param name="commission"></param>
        /// <returns></returns>
        private async Task CreateOrderCommission(Order order, List<Commission> commission, List<OrderCommission> orderCommissions)
        {
            if (commission?.Count == 0)
                return;

            var commissionPrice = commission?.Where(x => x.TotalPriceFrom < order.OrderTotal)
                                 .Select(x => x.CommissionPrice)
                                 .DefaultIfEmpty(0)
                                 .Max();

            if (commissionPrice == 0)
                return;

            var orderCommission = orderCommissions.FirstOrDefault(x => x.OrderId == order.Id && x.EmployeeId == order.CreateEmployeeId);

            if (orderCommission == null)
            {
                if (order.OrderSourceId == OrderConstants.ORDER_SOURCE_NORMAL)
                {
                    var orderCommissionTemp = new OrderCommission
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        OrderTotal = order.OrderTotal,
                        CreateDate = GetDateTimeNow(),
                        EmployeeId = order.CreateEmployeeId,
                        OrderCommission1 = commissionPrice.HasValue ? commissionPrice.Value : 0
                    };
                    await _dbContext.OrderCommissions.AddAsync(orderCommissionTemp);
                    orderCommissions.Add(orderCommissionTemp);
                }
                else if (order.OrderSourceId == OrderConstants.ORDER_SOURCE_TAKE_CARE)
                {
                    var orderCommissionTemp = new OrderCommission
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        OrderTotal = order.OrderTotal,
                        CreateDate = GetDateTimeNow(),
                        EmployeeId = order.CreateEmployeeId,
                        OrderCommission1 = (order.OrderTotal * OrderConstants.PERCENT_COMMISSION_TAKE_CARE) / 100
                    };
                    await _dbContext.OrderCommissions.AddAsync(orderCommissionTemp);
                    orderCommissions.Add(orderCommissionTemp);
                }
            }
            else
            {
                orderCommission.OrderCommission1 = commissionPrice.HasValue ? commissionPrice.Value : 0;
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderPaginationDto> GetOrdersWithPaginationAsync(DateTime startDate,
            DateTime endDate,
            Guid employeeCreateId,
            Guid customerId,
            int page,
            int pageSize,
            bool isGetWithoutDate,
            int statusOrderId,
            int sourceOrderId,
            int orderStatusPaymentId,
            int orderStatusShippingId,
            int orderShippingMethodId,
            string phone)
        {
            var orderPage = new OrderPaginationDto
            {
                Page = page,
                PageSize = pageSize,
                TotalOrder = BaseConstants.INT_DEFAULT,
                TotalPage = BaseConstants.INT_DEFAULT,
            };

            var orders = await _dbContext.Orders.AsNoTracking().OrderByDescending(x => x.OrderDate).ToListAsync();

            //fillter with orderStatus
            if(statusOrderId != BaseConstants.INT_DEFAULT)
            {
                orders = orders.Where(x => x.OrderStatusId == statusOrderId).ToList();
            }

            //fillter with status payment
            if (orderStatusPaymentId != BaseConstants.INT_DEFAULT)
            {
                orders = orders.Where(x => x.OrderStatusId == orderStatusPaymentId).ToList();
            }

            //fillter with source order
            if (sourceOrderId != BaseConstants.INT_DEFAULT)
            {
                orders = orders.Where(x => x.OrderStatusId == sourceOrderId).ToList();
            }

            //fillter with status shipping
            if (orderStatusShippingId != BaseConstants.INT_DEFAULT)
            {
                orders = orders.Where(x => x.OrderStatusId == orderStatusShippingId).ToList();
            }

            //fillter with shipping method
            if (orderShippingMethodId != BaseConstants.INT_DEFAULT)
            {
                orders = orders.Where(x => x.OrderStatusId == orderShippingMethodId).ToList();
            }

            //fillter with employee create
            if(employeeCreateId != Guid.Empty)
            {
                orders = orders.Where(x => x.CreateEmployeeId == employeeCreateId).ToList();
            }

            //fillter for date
            if (!isGetWithoutDate)
            {
                orders = orders.Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date).ToList();
            }

            //filter for phone 
            if (!string.IsNullOrEmpty(phone))
            {
                orders = orders.Where(x => x.CustomerPhone == phone).ToList();
            }

            //filter for phone 
            if (customerId != Guid.Empty)
            {
                orders = orders.Where(x => x.CustomerId == customerId).ToList();
            }

            var totalOrdersPerPage = orders.OrderByDescending(o => o.OrderDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalOrder = orders.Count();

            if (totalOrder == 0)
                return orderPage;

            orderPage.Page = page;
            orderPage.PageSize = pageSize;
            orderPage.TotalOrder = totalOrder;
            orderPage.TotalPage = (int)Math.Ceiling((double)totalOrder / pageSize);

            orderPage.Orders = await GetOrderWithOrderDetail(totalOrdersPerPage);

            return orderPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        private async Task<List<OrderDto>> GetOrderWithOrderDetail(List<Order> orders)
        {
            var ordersList = new List<OrderDto>();
            if (orders == null || !orders.Any())
                return ordersList;

            var orderDetails = await _dbContext.OrderDetails.AsNoTracking().Where(x => !x.IsDeleted && orders.Select(x => x.Id).Contains(x.OrderId)).ToListAsync();

            foreach (var order in orders)
            {
                var orderDto = MapFOrderTOrderDto(order);
                var orderDetailForOrder = orderDetails?.Where(x => x.OrderId == order.Id).ToList();

                if (orderDetailForOrder != null && orderDetailForOrder.Any())
                {
                    orderDto.products = orderDetailForOrder?.Select(x => MapFOrderDetailTOrderDetailDto(x)).ToList();
                }

                ordersList.Add(orderDto);
            }

            return ordersList;
        }

        public async Task<StatusOrderDto> GetAllOrderStatusAsync()
        {
            var statusShipping = await _dbContext.StatusShippings.ToListAsync();
            var orderSource = await _dbContext.OrderSources.ToListAsync();
            var statusPayment = await _dbContext.StatusPayments.ToListAsync();
            var statusOrder = await _dbContext.StatusOrders.ToListAsync();
            var shippingMethod = await _dbContext.ShippingMethods.ToListAsync();

            var statusOrderDto = new StatusOrderDto();

            foreach (var item in statusShipping)
            {
                statusOrderDto.StatusShipping.Add(new ModelStatusDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            foreach (var item in orderSource)
            {
                statusOrderDto.OrderSource.Add(new ModelStatusDto
                {
                    Id = item.Id,
                    Name = item.SourceName
                });
            }

            foreach (var item in statusPayment)
            {
                statusOrderDto.StatusPayment.Add(new ModelStatusDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            foreach (var item in statusOrder)
            {
                statusOrderDto.StatusOrder.Add(new ModelStatusDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            foreach (var item in shippingMethod)
            {
                statusOrderDto.ShippingMethod.Add(new ModelStatusDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return statusOrderDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDateAgo"></param>
        /// <param name="toDateAgo"></param>
        /// <returns></returns>
        public async Task<List<OrderDto>> GetOrdersToCallTakeCareWithDateAgoAsyns(int fromDateAgo, int toDateAgo)
        {
            DateTime startDate = GetDateTimeNow().AddDays(-toDateAgo);
            DateTime endDate = GetDateTimeNow().AddDays(-fromDateAgo);
            var orders = await _dbContext.Orders.AsNoTracking().Where(x => x.OrderDate.Date >= startDate.Date && x.OrderDate.Date <= endDate.Date && !x.IsRemovedCallTakeCare).OrderByDescending(x => x.OrderDate).ToListAsync();

            var orderDtos = await GetOrderWithOrderDetail(orders);

            return orderDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task RemoveCallTakeOrderAsync(Guid orderId)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId && !x.IsDeleted);

            if (order == null)
            {
                throw new BusinessException(OrderConstants.ORDER_NOT_EXISTS);
            }

            order.IsRemovedCallTakeCare = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
