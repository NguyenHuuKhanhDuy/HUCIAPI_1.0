using ApplicationCore.ViewModels.Brand;
using ApplicationCore.ViewModels.Catogory;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryVM categoryVM)
        {
            var category = await _categoryServices.CreateCategoryAsync(categoryVM);
            return HandleResponse(category, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateVM categoryVM)
        {
            var category = await _categoryServices.UpdateCategoryAsync(categoryVM);
            return HandleResponse(category, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            await _categoryServices.DeleteCategory(categoryId);
            return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryServices.GetAllCategoriesAsync();
            return HandleResponse(categories, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }
    }
}
