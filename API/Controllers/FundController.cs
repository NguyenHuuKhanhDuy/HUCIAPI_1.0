using ApplicationCore.ViewModels.Fund;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundController : BaseController
    {
        private readonly IFundServices _fundServices;
        private readonly ILogger _logger;
        public FundController(IFundServices fundServices, ILogger logger)
        {
            _fundServices = fundServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateFundAsync(FundVM fundVM)
        {
            _logger.LogInformation("Start create fund...");

            var fund = await _fundServices.CreateFundAsync(fundVM);

            _logger.LogInformation("End create fund...");

            return HandleResponseStatusOk(fund);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateFundAsync(FundUpdateVM fundVM)
        {
            _logger.LogInformation("Start update fund...");

            var fund = await _fundServices.UpdateFundAsync(fundVM);

            _logger.LogInformation("End update fund...");

            return HandleResponseStatusOk(fund);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteFundAsync(Guid fundId)
        {
            _logger.LogInformation($"Start delete fund id: {fundId}");

            await _fundServices.DeleteFundAsync(fundId);

            _logger.LogInformation($"End delete fund id: {fundId}");

            return HandleResponseStatusOk();
        }
    }
}
