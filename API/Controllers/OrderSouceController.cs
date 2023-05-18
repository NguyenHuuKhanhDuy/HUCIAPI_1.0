using ApplicationCore.ViewModels.OrderSource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if !DEBUG
    [Authorize]
#endif
    public class OrderSouceController : BaseController
    {
        private readonly IOrderSourceServices _orderSourceServices;
        private readonly ILogger _logger;
        public OrderSouceController(IOrderSourceServices orderSourceServices, ILogger<OrderSouceController> logger)
        {
            _orderSourceServices = orderSourceServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrderSourceAsync(OrderSourceVM vm)
        {
            _logger.LogInformation($"Start create order source... {GetStringFromJson(vm)}");

            var orderSource = await _orderSourceServices.CreateOrderSourceAsync(vm);

            _logger.LogInformation($"End create order source... {GetStringFromJson(orderSource)}");

            return HandleResponseStatusOk(orderSource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrderSourceAsync(OrderSourceUpdateVM vm)
        {
            _logger.LogInformation($"Start update order source... {GetStringFromJson(vm)}");

            var orderSource = await _orderSourceServices.UpdateOrderSourceAsync(vm);

            _logger.LogInformation($"End update order source... {GetStringFromJson(orderSource)}");

            return HandleResponseStatusOk(orderSource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSourceId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOrderSourceAsync(int orderSourceId)
        {
            _logger.LogInformation($"Start delete order source... {orderSourceId}");

            await _orderSourceServices.DeleteOrderSourceAsync(orderSourceId);

            _logger.LogInformation($"End delete order source... {orderSourceId}");

            return HandleResponseStatusOk();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllOrderSourceAsync()
        {
            _logger.LogInformation($"Start get all order source...");

            var orderSources = await _orderSourceServices.GetAllOrderSource();

            _logger.LogInformation($"End get all order source... {GetStringFromJson(orderSources)}");

            return HandleResponseStatusOk(orderSources);
        }
    }
}
