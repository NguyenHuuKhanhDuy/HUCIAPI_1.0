using ApplicationCore.ModelsDto.Product;

namespace ApplicationCore.ModelsDto.Order
{
    public class StatisticalOrderToday
    {
        public int TotalOrder { get; set; }

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public List<StatisticalProductToday> Products { get; set; } = new List<StatisticalProductToday>();
    }
}
