namespace ApplicationCore.ModelsDto.Product
{
    public class StatisticalProductToday
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public int Quantity { get; set; }
    }
}
