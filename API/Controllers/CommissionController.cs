using ApplicationCore.ViewModels.Commission;
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
    public class CommissionController : BaseController
    {
        private readonly ICommissionServices _commissionServices;
        private readonly ILogger _logger;
        public CommissionController(ICommissionServices commissionServices, ILogger<CommissionController> logger)
        {
            _commissionServices = commissionServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCommissionAsync(CommissionVM commissionVM)
        {
            _logger.LogInformation($"Start create commission: {GetStringFromJson(commissionVM)}");

            var commission = await _commissionServices.CreateComissionAsync(commissionVM);

            _logger.LogInformation("End create commission.");

            return HandleResponseStatusOk(commission);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCommissionAsync(CommissionUpdateVM commissionVM)
        {
            _logger.LogInformation($"Start update commission: {GetStringFromJson(commissionVM)}");

            var commission = await _commissionServices.UpdateComissionAsync(commissionVM);

            _logger.LogInformation("End update commission.");

            return HandleResponseStatusOk(commission);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionVM"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCommissionAsync(Guid commissionId)
        {
            _logger.LogInformation($"Start delete commission by id: {commissionId}");

            await _commissionServices.DeleteComissionByIdAsync(commissionId);

            _logger.LogInformation("End delete commission.");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCommissionAsync()
        {
            _logger.LogInformation($"Start get all commission...");

            var commissions = await _commissionServices.GetAllCommissionAsync();

            _logger.LogInformation($"End delete commission. {GetStringFromJson(commissions)}");

            return HandleResponseStatusOk(commissions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commissionId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCommissionByIdAsync(Guid commissionId)
        {
            _logger.LogInformation($"Start get commission by id: {commissionId}");

            var commission = await _commissionServices.GetAllCommissionAsync();

            _logger.LogInformation($"End get commission by id: {commissionId}");

            return HandleResponseStatusOk(commission);
        }
    }
}
