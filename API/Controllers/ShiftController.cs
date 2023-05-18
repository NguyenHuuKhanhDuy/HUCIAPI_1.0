using ApplicationCore.ViewModels.Shift;
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
    public class ShiftController : BaseController
    {
        private readonly IShiftServices _shiftServices;
        private readonly ILogger _logger;
        public ShiftController(IShiftServices shiftServices, ILogger<ShiftController> logger)
        {
            _shiftServices = shiftServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateShiftAsync(ShiftVM vm)
        {
            _logger.LogInformation($"Start create shift... {GetStringFromJson(vm)}");

            var shift = await _shiftServices.CreateShiftAsync(vm);

            _logger.LogInformation($"End create shift... {GetStringFromJson(shift)}");

            return HandleResponseStatusOk(shift);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateShiftAsync(ShiftUpdateVM vm)
        {
            _logger.LogInformation($"Start update shift... {GetStringFromJson(vm)}");

            var shift = await _shiftServices.UpdateShiftAsync(vm);

            _logger.LogInformation($"End update shift... {GetStringFromJson(shift)}");

            return HandleResponseStatusOk(shift);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteShiftAsync(Guid shiftId)
        {
            _logger.LogInformation($"Start delete shift... {shiftId}");

            await _shiftServices.DeleteShiftAsync(shiftId);

            _logger.LogInformation($"End delete shift... {shiftId}");

            return HandleResponseStatusOk();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllShiftAsync()
        {
            _logger.LogInformation($"Start get all shift...");

            var shifts = await _shiftServices.GetAllShiftAsync();

            _logger.LogInformation($"End get all shift... {GetStringFromJson(shifts)}");

            return HandleResponseStatusOk(shifts);
        }
    }
}
