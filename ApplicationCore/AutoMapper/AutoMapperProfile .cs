using ApplicationCore.ModelsDto;
using ApplicationCore.ModelsDto.Brand;
using ApplicationCore.ModelsDto.Category;
using ApplicationCore.ModelsDto.Product;
using ApplicationCore.ViewModels.Employee;
using ApplicationCore.ViewModels.Product;
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

            //product
            CreateMap<ProductVM, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, action => action.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductNumber, action => action.MapFrom(src => src.ProductNumber))
                .ForMember(dest => dest.Name, action => action.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, action => action.MapFrom(src => src.Price))
                .ForMember(dest => dest.WholesalePrice, action => action.MapFrom(src => src.WholesalePrice))
                .ForMember(dest => dest.Image, action => action.MapFrom(src => src.Image))
                .ForMember(dest => dest.Quantity, action => action.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IsActive, action => action.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.BrandId, action => action.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.CategoryId, action => action.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CreateDate, action => action.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.ProductTypeId, action => action.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.ProductTypeName, action => action.MapFrom(src => src.ProductTypeName))
                .ForMember(dest => dest.UserCreateId, action => action.MapFrom(src => src.UserCreateId))
                ;
        }
    }
}
