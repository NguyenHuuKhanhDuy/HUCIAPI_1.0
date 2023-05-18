using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.OrderSource;
using ApplicationCore.ViewModels.OrderSource;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class OrderSourceImp : BaseServices, IOrderSourceServices
    {
        private readonly HucidbContext _dbContext;
        public OrderSourceImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderSourceDto> CreateOrderSourceAsync(OrderSourceVM vm)
        {
            var orderSources = await _dbContext.OrderSources.ToListAsync();
            var orderSource = new OrderSource
            {
                Id = orderSources.Count + 1,
                SourceName = vm.SourceName,
                PercentCommission = 0,
            };

            await _dbContext.AddAsync(orderSource);
            await _dbContext.SaveChangesAsync();

            var dto = MapFOrderSourceTOrderSourceDto(orderSource);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteOrderSourceAsync(int orderSourceId)
        {
            var orderSource = await FindOrderSourceAsync(orderSourceId);
            _dbContext.OrderSources.Remove(orderSource);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<OrderSourceDto>> GetAllOrderSource()
        {
            var orderSources = await _dbContext.OrderSources.ToListAsync();
            var dtos = new List<OrderSourceDto>();

            foreach(var orderSource in orderSources)
            {
                dtos.Add(MapFOrderSourceTOrderSourceDto(orderSource));
            }

            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderSourceDto> UpdateOrderSourceAsync(OrderSourceUpdateVM vm)
        {
            var orderSource = await FindOrderSourceAsync(vm.Id);
            orderSource.SourceName = vm.SourceName;
            await _dbContext.SaveChangesAsync();

            var dto = MapFOrderSourceTOrderSourceDto(orderSource);

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSourceId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<OrderSource> FindOrderSourceAsync(int orderSourceId)
        {
            var orderSource = await _dbContext.OrderSources.FindAsync(orderSourceId);

            if(orderSource == null)
            {
                throw new BusinessException(OrderSourceConstant.OrderSourceNotExist);
            }

            return orderSource;
        }
    }
}
