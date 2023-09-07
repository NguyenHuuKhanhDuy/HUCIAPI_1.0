using ApplicationCore.ModelsDto.HistoryAction;

namespace Services.Interface
{
    public interface IHistoryAction
    {
        Task<List<HistoryActionDto>> GetHistoryAction(Guid id);
    }
}
