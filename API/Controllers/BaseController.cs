using ApplicationCore.Exceptions;
using ApplicationCore.Helper;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        //public IActionResult HandleException(Exception ex)
        //{
        //    DataResponse response = new DataResponse(null, StatusCodeConstants.STRING_EMPTY, StatusCodeConstants.STATUS_SUCCESS);
        //    switch (ex)
        //    {
        //        case ValidateException:
        //            response.status = StatusCodeConstants.STATUS_EXP_VALIDATE;
        //            response.massage = ex.Message;
        //            break;
        //        case BusinessException:
        //            response.status = StatusCodeConstants.STATUS_EXP_BUSINESS;
        //            response.massage = ex.Message;
        //            break;
        //        default:
        //            response.status = StatusCodeConstants.STATUS_INTERNAL_SERVER_ERROR;
        //            response.massage = ex.Message;
        //            break;
        //    }

        //    //response data for FE.
        //    return StatusCode(response.status, response);
        //}
    }
}
