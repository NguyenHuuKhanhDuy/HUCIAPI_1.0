using ApplicationCore.Helper;
using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels.Employee;
using ApplicationCore.ViewModels.Order;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IOrderServices _orderServices;
        private readonly ILogger _logger;

        public LoginController(IEmployeeServices employeeServices, ILogger<LoginController> logger, IOrderServices orderServices)
        {
            _employeeServices = employeeServices;
            _logger = logger;
            _orderServices = orderServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserVM userVM)
        {
            _logger.LogInformation($"Start login: {GetStringFromJson(userVM)}");

            int statusCode = StatusCodeConstants.STATUS_SUCCESS;

            if (userVM == null)
            {
                statusCode = StatusCodeConstants.STATUS_EXP_VALIDATE;
                return StatusCode(statusCode, new DataResponse(userVM, LoginConstants.USER_EMPTY, statusCode));
            }

            EmployeeDto employeeDto = await _employeeServices.Login(userVM);

            _logger.LogInformation($"End login: {GetStringFromJson(employeeDto)}");

            return StatusCode(statusCode, new DataResponse(employeeDto, StatusCodeConstants.MESSAGE_SUCCESS, statusCode));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOrderFromLadiPage([FromBody] OrderForLadipageVM orderVM) 
        {
            _logger.LogInformation($"Start create order from Ladipage: {GetStringFromJson(orderVM)}");
            //var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var order = await _orderServices.CreateOrderFromLadipageAsync(orderVM);

            _logger.LogInformation($"End create order from Ladipage: {GetStringFromJson(orderVM)}");

            return HandleResponseStatusOk(order);
        }
    }
}
