using ApplicationCore.ModelsDto.CallTakeCare;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.CallTakeCare;

namespace Services.Interface
{
    public interface ICallTakeCareServices
    {
        Task<CallTakeCareDto> CreateCallTakeCareAsync(CallTakeCareVM vm);
        Task<CallTakeCareDto> UpdateCallTakeCareAsync(CallTakeCareUpdateVM vm);
        Task DeleteCallTakeCareAsync(Guid callTakeCareId);
        Task<List<CallTakeCareDto>> GetAllCallTakeCaresByOrderIdAsync(Guid orderId);
        Task GetCallTakeCareForOrderDtos(List<OrderDto> orders);
    }
}
