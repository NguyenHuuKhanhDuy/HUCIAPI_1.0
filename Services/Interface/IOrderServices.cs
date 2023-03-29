using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<OrderDto> CreateOrderAsync(OrderVM orderVM);
        Task<OrderDto> UpdateOrderAsync(OrderVM orderVM);
        Task DeleteOrderAsync(Guid orderId);
    }
}
