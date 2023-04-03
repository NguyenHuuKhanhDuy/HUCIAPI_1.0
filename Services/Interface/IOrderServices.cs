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
        Task<List<OrderDto>> GetOrderByDate(DateTime startDate, DateTime endDate);
    }
}
