using ApplicationCore.ModelsDto.HistoryAction;
using Infrastructure.Models;

namespace Services.Interface
{
    public interface IHistoryAction
    {
        List<HistoryActionDto> GetHistoryAction(Guid id, List<HistoryAction> historyAction);
    }
}
