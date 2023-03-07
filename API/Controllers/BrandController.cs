using ApplicationCore.ViewModels.Brand;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IBrandServices _brandService;

        public BrandController(IBrandServices brandService)
        {  
            _brandService = brandService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandVM brandVM)
        {
            try
            {
                var brand = await _brandService.CreateBrandAsync(brandVM);
                return HandleResponse(brand, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandUpdateVM brandVM)
        {
            try
            {
                var brand = await _brandService.UpdateBrandAsync(brandVM);
                return HandleResponse(brand, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteBrand(Guid brandId)
        {
            try
            {
                await _brandService.DeleteBrandAsync(brandId);
                return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllBrand()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                return HandleResponse(brands, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
