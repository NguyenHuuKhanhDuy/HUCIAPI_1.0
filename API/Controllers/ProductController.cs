using ApplicationCore.Exceptions;
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
        public readonly ILogger _logger;
        public ProductController(IProductServices productServices, ILogger<ProductController> logger)
        {
            _productServices = productServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductVM productVM)
        {
            var product = await _productServices.CreateProductAsync(productVM);
            return HandleResponse(product, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productUpdateVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProductAsync(ProductUpdateVM productUpdateVM)
        {
            var product = await _productServices.UpdateProductAsync(productUpdateVM);
            return HandleResponse(product, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProductAsync(Guid productId)
        {
            await _productServices.DeleteProductAsync(productId);
            return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProductAsync()
        {
            _logger.LogInformation("Start Get all product....");

            var products = await _productServices.GetAllProductsAsync();

            _logger.LogInformation("End Get all product.");
            return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByIdAsync(Guid productId)
        {
            _logger.LogInformation($"Start get product by Id: {productId}....");

            var products = await _productServices.GetProductByIdAsync(productId);

            _logger.LogInformation("End Get product.");

            return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByBrandIdAsync(Guid brandId)
        {
            _logger.LogInformation($"Start get product by brand Id: {brandId}....");

            var products = await _productServices.GetProductByBrandIdAsync(brandId);

            _logger.LogInformation("End Get product.");

            return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByCategoryIdAsync(Guid categoryId)
        {
            _logger.LogInformation($"Start get product by category Id: {categoryId}....");

            var products = await _productServices.GetProductByCategoryIdAsync(categoryId);

            _logger.LogInformation("End Get product.");

            return HandleResponse(products, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataForCreateProductAsync()
        {
            _logger.LogInformation($"Start get data for add product....");

            var data = await _productServices.GetDataForCreateProductAsync();

            _logger.LogInformation($"End get data for add product....");

            return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        //For combo
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateComboAsync([FromBody] ComboVM comboVM)
        {
            _logger.LogInformation("Start Create Combo....");

            var comboDto = await _productServices.CreateComboAsync(comboVM);

            _logger.LogInformation("End Create Combo....");

            return HandleResponse(comboDto, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        //For combo
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateComboAsync([FromBody] ComboUpdateVM comboVM)
        {
            _logger.LogInformation("Start update Combo....");

            var comboDto = await _productServices.UpdateComboAsync(comboVM);

            _logger.LogInformation("End update Combo....");

            return HandleResponse(comboDto, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }
    }
}
