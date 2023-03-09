using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Category;
using ApplicationCore.ViewModels.Catogory;
using AutoMapper;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class CategoryImp : BaseServices, ICategoryServices
    {
        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryImp(HucidbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryVM categoryVM)
        {
            if(categoryVM.ParentId != Guid.Empty)
            {
                var tempCate = await _dbContext.Categories.FindAsync(categoryVM.ParentId);
                if(tempCate == null || tempCate.IsDeleted)
                {
                    throw new BusinessException(CategoryConstants.CATEGORY_PARENT_NOT_EXIST);
                }
            }

            var userCreate = await _dbContext.Employees.FindAsync(categoryVM.UserCreateId);

            if(userCreate == null || userCreate.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            List<Category> categories = await _dbContext.Categories.ToListAsync();
            CheckExistCategory(categoryVM.Name, categories);

            Category category = new Category();
            category.Id = Guid.NewGuid();
            category.Name = categoryVM.Name;
            category.ParentId = categoryVM.ParentId;
            category.UserCreateId = categoryVM.UserCreateId;
            category.IsDeleted = false;

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public void CheckExistCategory(string categoryName, List<Category> categories)
        {
            var category =  categories.Where(x => x.Name == categoryName && !x.IsDeleted).FirstOrDefault();
            if(category != null)
            {
                throw new BusinessException(CategoryConstants.EXIST_CATEGORY_NAME);
            }

        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryUpdateVM categoryVM)
        {
            var category = await _dbContext.Categories.FindAsync(categoryVM.Id);
            if(category == null || category.IsDeleted)
            {
                throw new BusinessException(CategoryConstants.CATEGORY_NOT_EXIST);
            }

            var categoryWithoutCurent = await _dbContext.Categories.Where(x => x.Id != category.Id).ToListAsync();
            CheckExistCategory(categoryVM.Name, categoryWithoutCurent);

            if (categoryVM.ParentId != Guid.Empty)
            {
                var tempCate = await _dbContext.Categories.FindAsync(categoryVM.ParentId);
                if (tempCate == null || tempCate.IsDeleted)
                {
                    throw new BusinessException(CategoryConstants.CATEGORY_PARENT_NOT_EXIST);
                }
            }

            category.ParentId = categoryVM.ParentId;
            category.Name = categoryVM.Name;

            await _dbContext.SaveChangesAsync();

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public  async Task DeleteCategory(Guid categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category == null || category.IsDeleted)
            {
                throw new BusinessException(CategoryConstants.CATEGORY_NOT_EXIST);
            }

            category.Name += BaseConstants.DELETE;
            category.IsDeleted = true;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            List<Category> categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();

            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            return categoriesDto;
        }
    }
}
