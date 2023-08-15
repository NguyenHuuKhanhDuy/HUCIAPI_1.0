using ApplicationCore.ModelsDto.Supplier;
using ApplicationCore.ViewModels.Import;

namespace ApplicationCore.ModelsDto.Import
{
    public class ImportDto
    {
        public Guid Id { get; set; }
        public string ImportNumber { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public int Total { get; set; }
        public int StatusImportId { get; set; }
        public string StatusImportName { get; set; } = null!;
        public Guid UserCreateId { get; set; }
        public string UserCreateName { get; set; } = null!;
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set;} = null!;

        public List<ImportDetailDto> Details { get; set; } = new List<ImportDetailDto>();
    }
}
