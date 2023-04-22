using ApplicationCore.Helper;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse(Object o, string massage, int statusCode)
        {
            return StatusCode(statusCode, new DataResponse(o, massage, statusCode));
        }

        protected IActionResult HandleResponseStatusOk(Object o = null)
        {
            return StatusCode(StatusCodeConstants.STATUS_SUCCESS, new DataResponse(o, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS));
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

        public static string GetStringFromJson(Object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
