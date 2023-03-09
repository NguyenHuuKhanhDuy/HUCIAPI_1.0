using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductVM productVM)
        {
            try
            {
                var product = await _productServices.CreateProductAsync(productVM);
                return HandleResponse(product, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS );
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
