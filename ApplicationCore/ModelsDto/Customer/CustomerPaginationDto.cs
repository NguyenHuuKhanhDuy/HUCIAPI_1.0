namespace ApplicationCore.ModelsDto.Customer
{
    public class CustomerPaginationDto
    {
        public int TotalCustomer { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public List<CustomerDto> customer { get; set; } = new List<CustomerDto>();
    }
}
