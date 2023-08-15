using Microsoft.VisualBasic;

namespace ApplicationCore.ModelsDto.Order
{
    public class OrderBySourceDtos
    {
        public string NameSource { get; set; } = null!;
        public int Total { get; set; } = 0;
        public int TotalPrice { get; set; } = 0;
        public int TotalPriceWaitingOrder { get; set; } = 0;
        public float AveragePrice { get; set; } = 0;
        public float OrderConversionRate { get; set; } = 0;
    }
}
