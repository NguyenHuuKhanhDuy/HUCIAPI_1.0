using ApplicationCore.ModelsDto;
using AutoMapper;
using Infrastructure.Models;

namespace ApplicationCore.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
        }
    }
}
