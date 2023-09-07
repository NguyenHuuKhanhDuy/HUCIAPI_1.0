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
    //[Authorize]

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
        public async Task<IActionResult> CreateOrderAsync(OrderVM orderVM, bool isSetOrderDate = false)
        {
            _logger.LogInformation("Start create order...");

            OrderDto order = await _orderServices.CreateOrderAsync(orderVM, isSetOrderDate);

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
            var userId = HttpContext.Items["UserId"];

            _logger.LogInformation("Start delete order...");

            await _orderServices.DeleteOrderAsync(orderId, Guid.Parse(userId.ToString()));

            _logger.LogInformation("End delete order...");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> RemoveCallTakeCareOrderAsync(Guid orderId)
        {
            _logger.LogInformation("Start remove call take care order...");

            await _orderServices.RemoveCallTakeOrderAsync(orderId);

            _logger.LogInformation("End remove call take care order...");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="excelFile"></param>
        /// <returns></returns>
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
        /// <param name="excelFile"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatusShippingEMSAsync(IFormFile excelFile)
        {
            _logger.LogInformation("Start update status for order from EMS");

            var file = await _orderServices.UpdateStatusShippingEMSAsync(excelFile);

            _logger.LogInformation("End update status for order from EMS");
            return HandleResponseStatusOk(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderForPaginationAsync(
            DateTime startDate, 
            DateTime endDate,
            Guid employeeCreateId,
            Guid customerId,
            Guid brandId,
            string? phone,
            string? search,
            int page = BaseConstants.PageDefault,
            int pageSize = BaseConstants.PageSizeDefault,
            bool isGetWithoutDate = true,
            int statusOrderId = 0, 
            int sourceOrderId = 0,
            int orderStatusPaymentId = 0,
            int orderStatusShippingId = 0,
            int orderShippingMethodId = 0,
            bool isGetOrderDeleted = false
            )
        {
            _logger.LogInformation($"Start get order with page: {page}, pageSize: {pageSize}");
            
            var orders = await _orderServices.GetOrdersWithPaginationAsync(startDate, endDate, employeeCreateId, customerId, brandId, page, pageSize, isGetWithoutDate, statusOrderId, sourceOrderId, orderStatusPaymentId, orderStatusShippingId, orderShippingMethodId, phone, search, isGetOrderDeleted);

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
        public async Task<IActionResult> GetOrderDetailById(Guid orderId)
        {
            _logger.LogInformation($"Start get order detail id: {orderId}");

            var orders = await _orderServices.GetDetailOrderByIdAsync(orderId);

            _logger.LogInformation($"End get order detail by id.");

            return HandleResponseStatusOk(orders);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetStatisticalTodayAsync()
        {
            _logger.LogInformation($"Start get Statistical Order : {DateTime.UtcNow.Date}...");

            var orders = await _orderServices.GetStatisticalTodayAsync();

            _logger.LogInformation($"End get Statistical Order : {GetStringFromJson(orders)}...");

            return HandleResponseStatusOk(orders);
        }
    }
}
