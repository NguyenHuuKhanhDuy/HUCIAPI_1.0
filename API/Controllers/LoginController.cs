using ApplicationCore.Helper;
using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels.Employee;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IEmployeeServices _employeeServices;

        public LoginController(IEmployeeServices employeeServices)
        {
            _employeeServices = employeeServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserVM userVM)
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
    }
}
