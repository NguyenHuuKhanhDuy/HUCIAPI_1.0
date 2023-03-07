using ApplicationCore.ModelsDto;
using ApplicationCore.ModelsDto.Brand;
using ApplicationCore.ModelsDto.Category;
using ApplicationCore.ViewModels.Employee;
using AutoMapper;
using Infrastructure.Models;

namespace ApplicationCore.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeVM, Employee>();
            CreateMap<EmployeeUpdateVM, EmployeeVM>();
            CreateMap<EmployeeUpdateVM, Employee>();

            //Brand
            CreateMap<Brand, BrandDto>();

            //Category
            CreateMap<Category, CategoryDto>();
        }
    }
}
