using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.Interface;
using ApplicationCore.ViewModels.OtherCost;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if !DEBUG
    [Authorize]
#endif
    public class OtherCostController : BaseController
    {
        private readonly IOtherCostServices _otherCostServices;
        private readonly ILogger _logger;
        public OtherCostController(IOtherCostServices otherCostServices, ILogger<OtherCostController> logger)
        {
            _otherCostServices = otherCostServices;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateOtherCostAsync(OtherCostVM vm)
        {
            _logger.LogInformation($"Start create other cost... {GetStringFromJson(vm)}");

            var otherCost = await _otherCostServices.CreateOtherCostAsync(vm);

            _logger.LogInformation($"End create other cost... {GetStringFromJson(otherCost)}");

            return HandleResponseStatusOk(otherCost);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateOtherCostAsync(OtherCostUpdateVM vm)
        {
            _logger.LogInformation($"Start update other cost... {GetStringFromJson(vm)}");

            var otherCost = await _otherCostServices.UpdateOtherCostAsync(vm);

            _logger.LogInformation($"End update other cost... {GetStringFromJson(otherCost)}");

            return HandleResponseStatusOk(otherCost);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteOtherCostAsync(Guid otherCostId)
        {
            _logger.LogInformation($"Start delete other cost with id: {otherCostId}");

            await _otherCostServices.DeleteOtherCostAsync(otherCostId);

            _logger.LogInformation($"End delete other cost with id: {otherCostId}");

            return HandleResponseStatusOk();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllOtherCostAsync()
        {
            _logger.LogInformation($"Start get all other cost...");

            var otherCosts = await _otherCostServices.GetAllOtherCostAsync();

            _logger.LogInformation($"Start get all other cost {GetStringFromJson(otherCosts)}");

            return HandleResponseStatusOk(otherCosts);
        }
    }
}
