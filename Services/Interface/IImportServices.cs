using ApplicationCore.ModelsDto.Import;
using ApplicationCore.ViewModels.Import;

namespace Services.Interface
{
    public interface IImportServices
    {
        Task<ImportDto> CreateImportAsync(ImportVM vm);
        Task<ImportDto> UpdateImportAsync(ImportUpdateVM vm);
        Task DeleteImportAsync(Guid Id);
        Task<ImportDto> GetImportByIdAsync(Guid Id);
        Task<List<ImportDto>> GetAllImportAsync();
    }
}
