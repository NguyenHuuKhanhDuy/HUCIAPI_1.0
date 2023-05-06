using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.Order;
using Common.Constants;
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
    public class OrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly ILogger _logger;
        public OrderController(IOrderServices orderServices, ILogger<OrderController> logger)
        {
            _orderServices = orderServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrderAsync(OrderVM orderVM)
        {
            _logger.LogInformation("Start create order...");

            OrderDto order = await _orderServices.CreateOrderAsync(orderVM);

            _logger.LogInformation("End create order...");

            return HandleResponseStatusOk(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOrderAsync(OrderUpdateVM orderVM)
        {
            _logger.LogInformation("Start update order...");

            var order = await _orderServices.UpdateOrderAsync(orderVM);

            _logger.LogInformation("End update order...");

            return HandleResponseStatusOk(order);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOrderAsync(Guid orderId)
        {
            _logger.LogInformation("Start delete order...");

            await _orderServices.DeleteOrderAsync(orderId);

            _logger.LogInformation("End delete order...");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderByDateAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Start get order from {startDate} to {endDate}");

            var orders = await _orderServices.GetOrderByDateAsync(startDate, endDate);

            _logger.LogInformation($"End get order from {startDate} to {endDate}");

            return HandleResponseStatusOk(orders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllOrderAsync()
        {
            _logger.LogInformation($"Start get all order...");

            var orders = await _orderServices.GetAllOrderAsync();

            _logger.LogInformation($"End get all order...");

            return HandleResponseStatusOk(orders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderByStatusId(int statusId)
        {
            _logger.LogInformation($"Start get order by status id: {statusId}");

            var orders = await _orderServices.GetOrderByStatusIdAsync(statusId);

            _logger.LogInformation($"End get order by status id.");

            return HandleResponseStatusOk(orders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderDetailById(Guid orderId)
        {
            _logger.LogInformation($"Start get order detail id: {orderId}");

            var orders = await _orderServices.GetDetailOrderByIdAsync(orderId);

            _logger.LogInformation($"End get order detail by id.");

            return HandleResponseStatusOk(orders);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatusShippingGHTKAsync(IFormFile excelFile)
        {
            _logger.LogInformation("Start update status for order from GHTK");

            var file = await _orderServices.UpdateStatusShippingGHTKAsync(excelFile);

            _logger.LogInformation("End update status for order from GHTK");
            return HandleResponseStatusOk(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderForPaginationAsync(int page = BaseConstants.PageDefault, int pageSize = BaseConstants.PageSizeDefault)
        {
            _logger.LogInformation($"Start get order with page: {page}, pageSize: {pageSize}");
            
            var orders = await _orderServices.GetOrdersWithPagination(page, pageSize);

            _logger.LogInformation($"End get order with page: {page}, pageSize: {pageSize} \r\n{GetStringFromJson(orders)}");

            return HandleResponseStatusOk(orders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllStatusForOrderAsync()
        {
            _logger.LogInformation("Start get all status for order");

            var status = await _orderServices.GetAllOrderStatusAsync();

            _logger.LogInformation($"End get all status for order. \r\n {GetStringFromJson(status)}");

            return HandleResponseStatusOk(status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDateAdo"></param>
        /// <param name="toDateAgo"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrdersToCallTakeCareWithDateAgoAsyns(int fromDateAdo, int toDateAgo)
        {
            _logger.LogInformation($"Start get order to call take care from {fromDateAdo} to {toDateAgo}");

            var orders = await _orderServices.GetOrdersToCallTakeCareWithDateAgoAsyns(fromDateAdo, toDateAgo);

            _logger.LogInformation($"End get order to call take care from {fromDateAdo} to {toDateAgo}. {GetStringFromJson(orders)}");

            return HandleResponseStatusOk(orders);
        }
    }  
}
