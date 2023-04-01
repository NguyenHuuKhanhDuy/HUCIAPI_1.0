using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly ILogger _logger;
        public OrderController(IOrderServices orderServices, ILogger<OrderController> logger)
        {
            _orderServices = orderServices;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrderAsync(OrderVM orderVM)
        {
            _logger.LogInformation("Start create order...");

            OrderDto order = await _orderServices.CreateOrderAsync(orderVM);

            _logger.LogInformation("End create order...");

            return HandleResponseStatusOk(order);
        }
    }
}
