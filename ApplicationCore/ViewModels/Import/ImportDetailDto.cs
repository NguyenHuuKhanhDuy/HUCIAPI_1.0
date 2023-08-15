namespace ApplicationCore.ViewModels.Import
{
    public class ImportDetailDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductNumber { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public int ProductPriceImport { get; set; }

        public int Quantity { get; set; }
    }
}
