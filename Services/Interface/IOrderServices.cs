using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<OrderDto> CreateOrderAsync(OrderVM orderVM);
        Task<OrderDto> UpdateOrderAsync(OrderUpdateVM orderVM);
        Task DeleteOrderAsync(Guid orderId);
        Task<List<OrderDto>> GetAllOrderAsync();
        Task<List<OrderDto>> GetOrderByDateAsync(DateTime startDate, DateTime endDate);
        Task<List<OrderDto>> GetOrderByStatusIdAsync(int statusId);
        Task<OrderDto> GetDetailOrderByIdAsync(Guid orderId);
        Task<OrderDto> CreateOrderFromLadipageAsync(OrderForLadipageVM orderVM);
    }
}
