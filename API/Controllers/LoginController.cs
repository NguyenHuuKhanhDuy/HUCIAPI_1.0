using ApplicationCore.Exceptions;
using ApplicationCore.Helper;
using ApplicationCore.ModelsDto;
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

        public LoginController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserVM userVM)
        {
            try
            {
                int statusCode = StatusCodeConstants.STATUS_SUCCESS;

                if (userVM == null)
                {
                    statusCode = StatusCodeConstants.STATUS_EXP_VALIDATE;
                    return StatusCode(statusCode, new DataResponse(userVM, LoginConstants.USER_EMPTY, statusCode));
                }

                EmployeeDto employeeDto = await _employeeServices.Login(userVM);

                return StatusCode(statusCode, new DataResponse(employeeDto, StatusCodeConstants.MESSAGE_SUCCESS, statusCode));
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }
    }
}
