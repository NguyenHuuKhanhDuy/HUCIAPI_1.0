using ApplicationCore.ViewModels.Promotion;
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
    public class PromotionController : BaseController
    {
        private IPromotionServices _promotionServices;
        private ILogger _logger;
        public PromotionController(IPromotionServices promotionServices, ILogger<PromotionController> logger)
        {
            _promotionServices = promotionServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePromotionAsync(PromotionVM promotionVM)
        {
            _logger.LogInformation("Start create promotion...");

            var promotion = await _promotionServices.CreatePromotionAsync(promotionVM);

            _logger.LogInformation("End create promotion...");

            return HandleResponseStatusOk(promotion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePromotionAsync(PromotionUpdateVM promotionVM)
        {
            _logger.LogInformation("Start update promotion...");

            var promotion = await _promotionServices.UpdatePromotionAsync(promotionVM);

            _logger.LogInformation("End update promotion...");

            return HandleResponseStatusOk(promotion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletePromotionAsync(Guid promotionId)
        {
            _logger.LogInformation("Start delete promotion...");

            await _promotionServices.DeletePromotionAsync(promotionId);

            _logger.LogInformation("End delete promotion...");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPromotionByIdAsync(Guid promotionId)
        {
            _logger.LogInformation($"Start get promotion by id: {promotionId}");

            var promotion = await _promotionServices.GetPromotionByIdAsync(promotionId);

            _logger.LogInformation($"End get promotion by id: {promotionId}");

            return HandleResponseStatusOk(promotion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPromotionAsync()
        {
            _logger.LogInformation($"Start get all promotion...");

            var promotions = await _promotionServices.GetAllPromotionAsync();

            _logger.LogInformation($"Start get all promotion...");

            return HandleResponseStatusOk(promotions);
        }
    }
}
