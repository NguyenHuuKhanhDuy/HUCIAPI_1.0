using ApplicationCore.ViewModels.Product;

namespace ApplicationCore.ViewModels.Import
{
    public class ImportVM
    {
        public int StatusImportId { get; set; }

        public Guid UserCreateId { get; set; }

        public Guid SupplierId { get; set; }

        public List<ProductInsideComboVM> Products { get; set; } = new List<ProductInsideComboVM>();
    }
}
