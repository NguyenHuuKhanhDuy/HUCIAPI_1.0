using ApplicationCore.ModelsDto.Category;
using ApplicationCore.ViewModels.Catogory;

namespace Services.Interface
{
    public interface ICategoryServices
    {
        Task<CategoryDto> CreateCategoryAsync(CategoryVM categoryVM);

        Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateVM categoryVM);
    }
}
