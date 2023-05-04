namespace ApplicationCore.ModelsDto.Order
{
    public class StatusOrderDto
    {
        public List<ModelStatusDto> OrderSource { get; set; } = new List<ModelStatusDto>();
        public List<ModelStatusDto> StatusShipping { get; set; } = new List<ModelStatusDto>();
        public List<ModelStatusDto> StatusPayment { get; set; } = new List<ModelStatusDto>();
        public List<ModelStatusDto> StatusOrder { get; set; } = new List<ModelStatusDto>();
        public List<ModelStatusDto> ShippingMethod { get; set; } = new List<ModelStatusDto>();
    }
}
