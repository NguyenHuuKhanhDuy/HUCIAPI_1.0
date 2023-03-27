using ApplicationCore.ModelsDto.Supplier;
using ApplicationCore.ViewModels.Supplier;

namespace Services.Interface
{
    public interface ISupplierServices
    {
        Task<SupplierDto> CreateCustomerAsync(SupplierVM SupplierVM);
        Task<SupplierDto> UpdateCustomerAsync(SupplierUpdateVM supplierVM);
        Task DeleteSupplierAsync(Guid supplierId);
        Task<SupplierDto> GetSupplierByIdAsync(Guid supplierId);
        Task<List<SupplierDto>> GetAllSupplierrAsync();
    }
}
