using ApplicationCore.Exceptions;
using Common.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Helper.HandleException
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError($"Something wrong: {exception}");
            DataResponse response = new DataResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            switch (exception)
            {
                case ValidateException:
                    response.status = StatusCodeConstants.STATUS_EXP_VALIDATE;
                    response.message = exception.Message;
                    break;
                case BusinessException:
                    response.status = StatusCodeConstants.STATUS_INTERNAL_SERVER_ERROR;
                    response.message = exception.Message;
                    break;
                default:
                    response.status = StatusCodeConstants.STATUS_INTERNAL_SERVER_ERROR;
                    response.message = exception.Message;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.status;
            await context.Response.WriteAsync(response.ToString());
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
