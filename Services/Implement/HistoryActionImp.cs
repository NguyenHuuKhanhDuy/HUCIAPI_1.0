using ApplicationCore.ModelsDto.HistoryAction;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
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

        public List<HistoryActionDto> GetHistoryAction(Guid id, List<HistoryAction> historyAction)
        {
            var actions = historyAction.Where(x => x.IdAction == id).ToList();
            var actionsDto = new List<HistoryActionDto>();

            foreach (var action in actions)
            {
                var dto = DataMapper.Map<HistoryAction, HistoryActionDto>(action);
                dto.UserCreateName = action.UserCreate.Name;
                actionsDto.Add(dto);
            }

            return actionsDto;
        }
    }
}
