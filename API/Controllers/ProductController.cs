using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductVM productVM)
        {
            try
            {
                var product = await _productServices.CreateProductAsync(productVM);
                return HandleResponse(product, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProductAsync(ProductUpdateVM productUpdateVM)
        {
            try
            {
                var product = await _productServices.UpdateProductAsync(productUpdateVM);
                return HandleResponse(product, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProductAsync(Guid productId)
        {
            try
            {
                await _productServices.DeleteProductAsync(productId);
                return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            try
            {
                var products = await _productServices.GetAllProductsAsync();
                return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var products = await _productServices.GetProductByIdAsync(productId);
                return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByBrandIdAsync(Guid brandId)
        {
            try
            {
                var products = await _productServices.GetProductByBrandIdAsync(brandId);
                return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByCategoryIdAsync(Guid categoryId)
        {
            try
            {
                var products = await _productServices.GetProductByCategoryIdAsync(categoryId);
                return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataForCreateProductAsync()
        {
            try
            {
                var data = await _productServices.GetDataForCreateProductAsync();
                return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
