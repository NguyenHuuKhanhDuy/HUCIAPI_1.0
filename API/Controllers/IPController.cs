using ApplicationCore.ViewModels.IP;
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
    public class IPController : BaseController
    {
        private readonly IIPServices _ipServices;
        private readonly ILogger _logger;
        public IPController(IIPServices ipServices, ILogger<IIPServices> logger)
        {
            _ipServices = ipServices;
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateIPAsync(IPVM vm)
        {
            _logger.LogInformation($"Start create ip... {GetStringFromJson(vm)}");

            var ip = await _ipServices.CreateIPAsync(vm);

            _logger.LogInformation($"End create ip... {GetStringFromJson(ip)}");

            return HandleResponseStatusOk(ip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateIPAsync(IPUpdateVM vm)
        {
            _logger.LogInformation($"Start update ip... {GetStringFromJson(vm)}");

            var ip = await _ipServices.UpdateIPAsync(vm);

            _logger.LogInformation($"End update ip... {GetStringFromJson(ip)}");

            return HandleResponseStatusOk(ip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteIPAsync(Guid IPId)
        {
            _logger.LogInformation($"Start delete ip... {IPId}");

            await _ipServices.DeleteIPAsync(IPId);

            _logger.LogInformation($"End delete ip... {IPId}");

            return HandleResponseStatusOk();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllIPAsync()
        {
            _logger.LogInformation($"Start get all ip...");

            var ips = await _ipServices.GetAllIPsAsync();

            _logger.LogInformation($"End get all ip... {GetStringFromJson(ips)}");

            return HandleResponseStatusOk(ips);
        }
    } 
}
