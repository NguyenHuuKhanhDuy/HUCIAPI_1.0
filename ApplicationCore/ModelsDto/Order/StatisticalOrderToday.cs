using ApplicationCore.ModelsDto.Product;

namespace ApplicationCore.ModelsDto.Order
{
    public class StatisticalOrderToday
    {
        public OrderBySourceDtos TotalOrder { get; set; } = new OrderBySourceDtos();
        public List<OrderBySourceDtos> OrderBySources { get; set; } = new List<OrderBySourceDtos>();

        public List<StatisticalProductToday> Products { get; set; } = new List<StatisticalProductToday>();
    }
}
