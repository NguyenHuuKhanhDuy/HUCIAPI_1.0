using ApplicationCore.ModelsDto.Customer;
using ApplicationCore.ViewModels.Customer;

namespace Services.Interface
{
    public interface ICustomerServices
    {
        Task<CustomerDto> CreateCustomerAsync(CustomerVM customerVM);
        Task<CustomerDto> UpdateCustomerAsync(CustomerUpdateVM customerVM);
        Task DeleteCustomerAsync(Guid customerId);
        Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
        Task<CustomerPaginationDto> GetCustomerForPagination(int Page, int PageSize);
    }
}
