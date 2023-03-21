using ApplicationCore.ViewModels.Customer;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

    }
}
