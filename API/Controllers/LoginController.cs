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
        //private readonly ILogger _logger;

        public LoginController(IEmployeeServices employeeServices/*, ILogger logger*/)
        {
            _employeeServices = employeeServices;
            //_logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(UserVM userVM)
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
                DataResponse response = new DataResponse(null, StatusCodeConstants.STRING_EMPTY, StatusCodeConstants.STATUS_SUCCESS);
                switch (ex)
                {
                    case ValidateException:
                        response.status = StatusCodeConstants.STATUS_EXP_VALIDATE;
                        response.massage = ex.Message;
                        break;
                    case BusinessException:
                        response.status = StatusCodeConstants.STATUS_EXP_BUSINESS;
                        response.massage = ex.Message;
                        break;
                    default:
                        response.status = StatusCodeConstants.STATUS_INTERNAL_SERVER_ERROR;
                        response.massage = ex.Message;
                        break;
                }

                //response data for FE.
                return StatusCode(StatusCodeConstants.STATUS_SUCCESS, response);
            }
        }
    }
}
