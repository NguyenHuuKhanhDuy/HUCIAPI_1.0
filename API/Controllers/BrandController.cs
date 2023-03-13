using ApplicationCore.ViewModels.Brand;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : BaseController
    {
        private readonly IBrandServices _brandService;

        public BrandController(IBrandServices brandService)
        {  
            _brandService = brandService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandVM brandVM)
        {
                var brand = await _brandService.CreateBrandAsync(brandVM);
                return HandleResponse(brand, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandUpdateVM brandVM)
        {
                var brand = await _brandService.UpdateBrandAsync(brandVM);
                return HandleResponse(brand, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteBrand(Guid brandId)
        {
                await _brandService.DeleteBrandAsync(brandId);
                return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBrand()
        {
                var brands = await _brandService.GetAllBrandsAsync();
                return HandleResponse(brands, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }
    }
}
