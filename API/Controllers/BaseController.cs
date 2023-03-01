using ApplicationCore.Exceptions;
using ApplicationCore.Helper;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public IActionResult HandleException(Exception ex)
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
            return StatusCode(response.status, response);
        }

        public static IActionResult ValidateModelState(ActionContext context)
        {
            (string fieldName, ModelStateEntry entry) = context.ModelState
                .First(x => x.Value.Errors.Count > 0);
            string errorSerialized = entry.Errors.First().ErrorMessage;

            
            DataResponse response = new DataResponse(null, errorSerialized, StatusCodeConstants.STATUS_EXP_VALIDATE);
            var result = new BadRequestObjectResult(response);

            return result;
        }
    }
}
