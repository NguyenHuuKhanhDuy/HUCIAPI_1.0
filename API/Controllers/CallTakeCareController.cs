using ApplicationCore.ViewModels.CallTakeCare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if !DEBUG
    [Authorize]
#endif
    public class CallTakeCareController : BaseController
    {
        private readonly ICallTakeCareServices _callTakeCareServices;
        private readonly ILogger _logger;
        public CallTakeCareController(ICallTakeCareServices callTakeCareServices, ILogger<CallTakeCareController> logger)
        {
            _callTakeCareServices = callTakeCareServices;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCallTakeCareAsync(CallTakeCareVM vm)
        {
            _logger.LogInformation($"Start create call take care... {GetStringFromJson(vm)}");

            var callTakeCare = await _callTakeCareServices.CreateCallTakeCareAsync(vm);

            _logger.LogInformation($"Start create call take care... {GetStringFromJson(callTakeCare)}");

            return HandleResponseStatusOk(callTakeCare);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCallTakeCareAsync(CallTakeCareUpdateVM vm)
        {
            _logger.LogInformation($"Start update call take care... {GetStringFromJson(vm)}");

            var callTakeCare = await _callTakeCareServices.UpdateCallTakeCareAsync(vm);

            _logger.LogInformation($"Start update call take care... {GetStringFromJson(callTakeCare)}");

            return HandleResponseStatusOk(callTakeCare);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCallTakeCareAsync(Guid callTakeCareId)
        {
            _logger.LogInformation($"Start delete call take care with id: {callTakeCareId}");

            await _callTakeCareServices.DeleteCallTakeCareAsync(callTakeCareId);

            _logger.LogInformation($"End delete call take care with id: {callTakeCareId}");

            return HandleResponseStatusOk();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCallTakeCaresByOrderIdAsync(Guid orderId)
        {
            _logger.LogInformation($"Start get all call take care with orderId: {orderId}");

            var callTakeCares = await _callTakeCareServices.GetAllCallTakeCaresByOrderIdAsync(orderId);

            _logger.LogInformation($"End get all call take care with orderId: {orderId} {GetStringFromJson(callTakeCares)}");

            return HandleResponseStatusOk(callTakeCares);
        }
    }
}
