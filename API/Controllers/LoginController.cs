using ApplicationCore.Helper;
using ApplicationCore.ViewModels;
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public LoginController(IEmployeeServices employeeServices, IConfiguration config, ILogger logger)
        {
            _employeeServices = employeeServices;
            _config = config;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(UserVM userVM)
        {
            int statusCode = StatusCodeConstants.STATUS_SUCCESS;
            if (userVM == null)
            {
                statusCode = StatusCodeConstants.STATUS_EXP_VALIDATE;
                return StatusCode(statusCode, new DataResponse(userVM, LoginConstants.USER_EMPTY, statusCode));
            }

            var employeeDto = 

            return Ok();
        }
    }
}
