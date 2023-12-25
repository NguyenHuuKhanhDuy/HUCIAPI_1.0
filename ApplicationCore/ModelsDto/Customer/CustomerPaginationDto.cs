using ApplicationCore.Helper.Models;

namespace ApplicationCore.ModelsDto.Customer
{
    public class CustomerPaginationDto : PagingResult
    {
        public List<CustomerDto> customer { get; set; } = new List<CustomerDto>();
    }
}
