using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Brand;
using ApplicationCore.ViewModels.Brand;
using AutoMapper;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class BrandImp : BaseServices, IBrandServices
    {
        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        public BrandImp(HucidbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// Create new brand
        /// </summary>
        /// <param name="brandVM"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<BrandDto> CreateBrandAsync(BrandVM brandVM)
        {
            var brands = await _dbContext.Brands.ToListAsync();
            await CheckInforBrand(brandVM.Name, brands, brandVM.UserCreateId);

            Brand brand = new Brand();
            brand.Id = Guid.NewGuid();
            brand.Name = brandVM.Name;
            brand.UserCreateId = brandVM.UserCreateId;
            brand.CreateDate= DateTime.UtcNow.AddHours(7);
            brand.IsDeleted = false;

            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();

            BrandDto brandDto = _mapper.Map<BrandDto>(brand);

            return brandDto;
        }

        /// <summary>
        /// Update brand
        /// </summary>
        /// <param name="brandVM"></param>
        /// <returns></returns>
        public async Task<BrandDto> UpdateBrandAsync(BrandUpdateVM brandVM)
        {
            Brand brand = await _dbContext.Brands.FindAsync(brandVM.Id);
            if(brand == null || brand.IsDeleted)
            {
                throw new BusinessException(BrandConstants.BRAND_NOT_EXIST);
            }

            var brands = await _dbContext.Brands.Where(x => x.Id != brandVM.Id).ToListAsync();
            await CheckInforBrand(brandVM.Name, brands);

            brand.Name= brandVM.Name;
            await _dbContext.SaveChangesAsync();

            BrandDto brandDto = _mapper.Map<BrandDto>(brand);

            return brandDto;
        }

        public async Task DeleteBrandAsync(Guid brandId)
        {
            Brand brand = await _dbContext.Brands.FindAsync(brandId);
            if (brand == null || brand.IsDeleted)
            {
                throw new BusinessException(BrandConstants.BRAND_NOT_EXIST);
            }

            brand.Name= brand.Name + BaseConstants.DELETE;
            brand.IsDeleted= true;

            await _dbContext.SaveChangesAsync();
        }

        
        public async Task CheckInforBrand(string brandName, List<Brand> brands, Guid? userCreateId = null)
        {
            bool checkExistName = brands.Where(x => x.Name == brandName && !x.IsDeleted).Any();
            if (checkExistName)
            {
                throw new BusinessException(BrandConstants.EXIST_BRAND_NAME);
            }

            if(userCreateId!= null)
            {
                var userCreate = await _dbContext.Employees.FindAsync(userCreateId);
                if (userCreate == null || userCreate.IsDeleted)
                {
                    throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
                }
            }
        }

        public async Task<List<BrandDto>> GetAllBrandsAsync()
        {
            List<Brand> brands = await _dbContext.Brands.Where(x => !x.IsDeleted).ToListAsync();

            List<BrandDto> result = _mapper.Map<List<BrandDto>>(brands);

            return result;
        }
    }
}
