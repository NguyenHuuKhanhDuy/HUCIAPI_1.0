using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;
using Common.Constants;
using Microsoft.AspNetCore.Http;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<OrderDto> CreateOrderAsync(OrderVM orderVM, bool isSetOtherDate, bool isWholeSale = false);
        Task<OrderDto> UpdateOrderAsync(OrderUpdateVM orderVM, bool isWholeSaleOrder = false);
        Task DeleteOrderAsync(Guid orderId, Guid userId);
        Task RemoveCallTakeOrderAsync(Guid orderId);
        Task<List<OrderDto>> GetAllOrderAsync();
        Task<List<OrderDto>> GetOrderByDateAsync(DateTime startDate, DateTime endDate);
        Task<OrderDto> GetDetailOrderByIdAsync(Guid orderId);
        Task<OrderDto> CreateOrderFromLadipageAsync(OrderForLadipageVM orderVM);
        Task<string> UpdateStatusShippingGHTKAsync(IFormFile excelFile);
        Task<string> UpdateStatusShippingEMSAsync(IFormFile excelFile);
        Task<OrderPaginationDto> GetOrdersWithPaginationAsync(DateTime date,
            Guid employeeCreateId,
            Guid customerId,
            Guid brandId,
            int page,
            int pageSize,
            bool isGetWithoutDate,
            int statusOrderId,
            int sourceOrderId,
            int orderShippingMethodId,
            string phone,
            string search,
            bool isGetOrderDeleted);
        Task<StatusOrderDto> GetAllOrderStatusAsync();
        Task<List<OrderDto>> GetOrdersToCallTakeCareWithDateAgoAsyns(int fromDateAgo, int toDateAgo);
        Task<StatisticalOrderToday> GetStatisticalTodayAsync();
        Task UpSaleOrderAsync(Guid orderId, Guid userId, bool isUpSaleOrder);
    }
}
