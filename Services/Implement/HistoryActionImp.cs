using ApplicationCore.ModelsDto.HistoryAction;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System.Collections.Generic;

namespace Services.Implement
{
    public class HistoryActionImp : BaseServices, IHistoryAction
    {
        private readonly HucidbContext _dbContext;
        public HistoryActionImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HistoryActionDto>> GetHistoryAction(Guid id)
        {
            var actions = await _dbContext.HistoryActions.Where(x => x.IdAction == id).ToListAsync();
            var employees = await _dbContext.Employees.ToListAsync();
            var actionsDto = new List<HistoryActionDto>();

            foreach (var action in actions)
            {
                var dto = Map<HistoryAction, HistoryActionDto>(action);
                var employee = employees.FirstOrDefault(x => x.Id == action.UserCreateId);
                dto.UserCreateName = employee.Name;
                actionsDto.Add(dto);
            }

            return actionsDto;
        }
    }
}
