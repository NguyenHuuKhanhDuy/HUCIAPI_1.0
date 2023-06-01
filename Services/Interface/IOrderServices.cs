using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;
using Common.Constants;
using Microsoft.AspNetCore.Http;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<OrderDto> CreateOrderAsync(OrderVM orderVM, bool isSetOtherDate);
        Task<OrderDto> UpdateOrderAsync(OrderUpdateVM orderVM);
        Task DeleteOrderAsync(Guid orderId);
        Task RemoveCallTakeOrderAsync(Guid orderId);
        Task<List<OrderDto>> GetAllOrderAsync();
        Task<List<OrderDto>> GetOrderByDateAsync(DateTime startDate, DateTime endDate);
        Task<List<OrderDto>> GetOrderByStatusIdAsync(int statusId);
        Task<OrderDto> GetDetailOrderByIdAsync(Guid orderId);
        Task<OrderDto> CreateOrderFromLadipageAsync(OrderForLadipageVM orderVM);
        Task<string> UpdateStatusShippingGHTKAsync(IFormFile excelFile);
        Task<string> UpdateStatusShippingEMSAsync(IFormFile excelFile);
        Task<OrderPaginationDto> GetOrdersWithPaginationAsync(DateTime startDate,
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
            string phone);
        Task<StatusOrderDto> GetAllOrderStatusAsync();
        Task<List<OrderDto>> GetOrdersToCallTakeCareWithDateAgoAsyns(int fromDateAgo, int toDateAgo);

    }
}
