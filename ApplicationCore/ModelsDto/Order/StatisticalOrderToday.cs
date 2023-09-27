using ApplicationCore.ModelsDto.Product;

namespace ApplicationCore.ModelsDto.Order
{
    public class StatisticalOrderToday
    {
        public OrderBySourceDtos TotalOrder { get; set; } = new OrderBySourceDtos();
        public List<OrderBySourceDtos> OrderBySources { get; set; } = new List<OrderBySourceDtos>();
        public List<StatisticalProductToday> Products { get; set; } = new List<StatisticalProductToday>();
        public List<StatisticalByRole> Roles { get; set; } = new List<StatisticalByRole>();
    }

    public class OrderBySourceDtos
    {
        public string NameSource { get; set; } = null!;
        public int Total { get; set; } = 0;
        public int TotalPrice { get; set; } = 0;
        public int TotalPriceWaitingOrder { get; set; } = 0;
        public int AveragePrice { get; set; } = 0;
        public int OrderConversionRate { get; set; } = 0;
        public List<CountOrderByStatus> Count { get; set; } = new List<CountOrderByStatus>();
    }

    public class StatisticalProductToday
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public int Quantity { get; set; }
    }

    public class CountOrderByStatus
    {
        public string StatusName { get; set; } = null!;
        public int Total { get; set; }
    }

    public class StatisticalByRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public List<StatisticalEmployee> Employee { get; set; } = new List<StatisticalEmployee>();
    }

    public class StatisticalEmployee
    {
        public string EmployeeName { get; set; } = null!;
        public int TotalOrder { get; set; } = 0;
        public int AveragePrice { get; set; } = 0;
        public UpSale UpSale { get; set; } = new UpSale();
        public UpSale Tranfer { get; set; } = new UpSale();
    }
    public class UpSale
    {
        public int Percent { get; set; } = 0;
        public int Call { get; set; } = 0;
        public int Deal { get; set; } = 0;
    }
}
