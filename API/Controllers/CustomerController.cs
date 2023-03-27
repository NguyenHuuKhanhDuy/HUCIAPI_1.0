using ApplicationCore.ViewModels.Customer;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerServices _customerServices;
        private readonly ILogger _logger;
        public CustomerController(ICustomerServices customerServices, ILogger<CustomerController> logger)
        {
            _customerServices = customerServices;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCustomerAsync(CustomerVM customerVM)
        {
            _logger.LogInformation("Start create customer...");

            var customer = await _customerServices.CreateCustomerAsync(customerVM);

            _logger.LogInformation("End create customer...");

            return HandleResponse(customer, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCustomerAsync(CustomerUpdateVM customerVM)
        {
            _logger.LogInformation("Start update customer...");

            var customer = await _customerServices.UpdateCustomerAsync(customerVM);

            _logger.LogInformation("End update customer...");

            return HandleResponse(customer, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DaleteCustomerAsync(Guid customerId)
        {
            _logger.LogInformation("Start delete customer...");

            await _customerServices.DeleteCustomerAsync(customerId);

            _logger.LogInformation("End delete customer...");

            return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCustomerByIdAsync(Guid customerId)
        {
            _logger.LogInformation($"Start get customer id: {customerId}");

            var customer = await _customerServices.GetCustomerByIdAsync(customerId);

            _logger.LogInformation($"End get customer id: {customerId}");

            return HandleResponse(customer, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCustomerAsync()
        {
            _logger.LogInformation($"Start get all customer ");

            var customer = await _customerServices.GetAllCustomerAsync();

            _logger.LogInformation($"End get all customer");

            return HandleResponse(customer, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);

        }
    }
}
