using ApplicationCore.ViewModels.Brand;
using ApplicationCore.ViewModels.Catogory;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryVM categoryVM)
        {
            try
            {
                var category = await _categoryServices.CreateCategoryAsync(categoryVM);
                return HandleResponse(category, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateVM categoryVM)
        {
            try
            {
                var category = await _categoryServices.UpdateCategoryAsync(categoryVM);
                return HandleResponse(category, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                await _categoryServices.DeleteCategory(categoryId);
                return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = await _categoryServices.GetAllCategoriesAsync();
                return HandleResponse(categories, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
