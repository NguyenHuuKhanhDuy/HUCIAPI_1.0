using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.CallTakeCare;
using ApplicationCore.ModelsDto.Order;
using ApplicationCore.ViewModels.CallTakeCare;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class CallTakeCareImp : BaseServices, ICallTakeCareServices
    {
        private readonly HucidbContext _dbContext;
        public CallTakeCareImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<CallTakeCareDto> CreateCallTakeCareAsync(CallTakeCareVM vm)
        {
            var employee = await _dbContext.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vm.UserCreateId && !x.IsDeleted);

            if (employee == null)
            {
                throw new BusinessException(TimeKeepingConstants.UserCreateNotExist);
            }

            var order = await _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == vm.OrderId);

            if (order == null)
            {
                throw new BusinessException(OrderConstants.ORDER_NOT_EXISTS);
            }

            var callTakeCare = new OrderTakeCare
            {
                Id = Guid.NewGuid(),
                IsDeleted = BaseConstants.IsDeletedDefault,
                CreateDate = GetDateTimeNow(),
                OrderId = vm.OrderId,
                Notes = vm.Notes,
                UserCreateId = vm.UserCreateId
            };

            await _dbContext.OrderTakeCares.AddAsync(callTakeCare);
            await _dbContext.SaveChangesAsync();

            var callTakeCareDto = MapFOrderTakeCareTCallTakeCareDto(callTakeCare, employee.Name);

            return callTakeCareDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<CallTakeCareDto> UpdateCallTakeCareAsync(CallTakeCareUpdateVM vm)
        {
            var callTakeCare = await _dbContext.OrderTakeCares.Include(x => x.UserCreate).FirstOrDefaultAsync(x => x.Id == vm.Id && !x.IsDeleted);
            
            if (callTakeCare == null)
            {
                throw new BusinessException(CallTakeCareConstants.OrderTakeCareNotExist);
            }

            callTakeCare.Notes = vm.Notes;

            await _dbContext.SaveChangesAsync();

            var callTakeCareDto = MapFOrderTakeCareTCallTakeCareDto(callTakeCare, callTakeCare.UserCreate.Name);

            return callTakeCareDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callTakeCareId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task DeleteCallTakeCareAsync(Guid callTakeCareId)
        {
            var callTakeCare = await _dbContext.OrderTakeCares.FirstOrDefaultAsync(x => x.Id == callTakeCareId && !x.IsDeleted);

            if (callTakeCare == null)
            {
                throw new BusinessException(CallTakeCareConstants.OrderTakeCareNotExist);
            }

            callTakeCare.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<CallTakeCareDto>> GetAllCallTakeCaresByOrderIdAsync(Guid orderId)
        {
            var callTakeCares = await _dbContext.OrderTakeCares.Include(x => x.UserCreate).AsNoTracking().Where(x => x.OrderId == orderId && !x.IsDeleted).OrderByDescending(x => x.CreateDate).ToListAsync();
            var employees = await _dbContext.Employees.AsNoTracking().ToListAsync();

            var callTakeCareDtos = new List<CallTakeCareDto>();

            foreach (var item in callTakeCares)
            {
                callTakeCareDtos.Add(MapFOrderTakeCareTCallTakeCareDto(item, item.UserCreate.Name));
            }

            return callTakeCareDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<CallTakeCareDto> GetAllCallTakeCaresByOrderId(List<OrderTakeCare> callTakeCares)
        {
            var callTakeCareDtos = new List<CallTakeCareDto>();

            foreach (var item in callTakeCares)
            {
                callTakeCareDtos.Add(MapFOrderTakeCareTCallTakeCareDto(item, item.UserCreate.Name));
            }

            return callTakeCareDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public async Task GetCallTakeCareForOrderDtos(List<OrderDto> orders)
        {
            var callTakeCares = await _dbContext.OrderTakeCares.Include(x => x.UserCreate).AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();

            if (!callTakeCares.Any())
                return;
            foreach (var item in orders)
            {
                item.CallTakeCares = GetAllCallTakeCaresByOrderId(callTakeCares.Where(x => x.OrderId == item.Id).ToList());
            }
        }
    }
}
