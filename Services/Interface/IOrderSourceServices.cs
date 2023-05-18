using ApplicationCore.ModelsDto.OrderSource;
using ApplicationCore.ViewModels.OrderSource;

namespace Services.Interface
{
    public interface IOrderSourceServices
    {
        Task<OrderSourceDto> CreateOrderSourceAsync(OrderSourceVM vm);
        Task<OrderSourceDto> UpdateOrderSourceAsync(OrderSourceUpdateVM vm);
        Task DeleteOrderSourceAsync(int orderSourceId);
        Task<List<OrderSourceDto>> GetAllOrderSource();
    }
}
