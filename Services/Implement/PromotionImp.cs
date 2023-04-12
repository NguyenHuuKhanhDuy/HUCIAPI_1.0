using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Promotion;
using ApplicationCore.ViewModels.Promotion;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class PromotionImp : BaseServices, IPromotionServices
    {
        private readonly HucidbContext _dbContext;
        public PromotionImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PromotionDto> CreatePromotionAsync(PromotionVM promotionVM)
        {
            var promotion = MapFPromotionVMToPromotion(promotionVM);

            promotion.Id = Guid.NewGuid();
            promotion.UserCreateName = await CheckInforPromotion(promotion.ProductId, promotion.UserCreateId);
            promotion.CreateDate = GetDateTimeNow();
            promotion.IsDeleted = false;

            await _dbContext.AddAsync(promotion);
            await _dbContext.SaveChangesAsync();

            var dto = MapFPromotionTPromotionDto(promotion);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<string> CheckInforPromotion(Guid productId, Guid userId)
        {
            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == productId && !x.IsDeleted);

            if (product == null)
            {
                throw new BusinessException(ProductConstants.PRODUCT_NOT_EXIST);
            }

            var user = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);

            if (user == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            return user.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeletePromotionAsync(Guid promotionId)
        {
            var promotion = await _dbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionId && !x.IsDeleted);

            if (promotion == null)
            {
                throw new BusinessException(PromotionConstants.PROMOTION_NOT_EXIST);
            }

            promotion.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PromotionDto>> GetAllPromotionAsync()
        {
            var promotions = await _dbContext.Promotions.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
            var promotionDtos = new List<PromotionDto>();

            if (promotions.Any())
            {
                promotionDtos = promotions.Select(x => MapFPromotionTPromotionDto(x)).OrderBy(x => x.CreateDate).ToList();
            }

            return promotionDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PromotionDto> GetPromotionByIdAsync(Guid promotionId)
        {
            var promotion = await _dbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionId && !x.IsDeleted);
            if(promotion == null)
            {
                throw new BusinessException(PromotionConstants.PROMOTION_NOT_EXIST);
            }

            var dto = MapFPromotionTPromotionDto(promotion);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="promotionVM"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PromotionDto> UpdatePromotionAsync(PromotionUpdateVM promotionVM)
        {
            var promotion = await _dbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionVM.Id && !x.IsDeleted);

            if(promotion == null)
            {
                throw new BusinessException(PromotionConstants.PROMOTION_NOT_EXIST);
            }

            MapFPromotionUpdateVMTPromotion(promotion, promotionVM);
            await _dbContext.SaveChangesAsync();

            var dto = MapFPromotionTPromotionDto(promotion);
            return dto;
        }
    }
}
