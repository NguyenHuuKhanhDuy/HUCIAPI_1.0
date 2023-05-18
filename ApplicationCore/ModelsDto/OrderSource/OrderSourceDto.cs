namespace ApplicationCore.ModelsDto.OrderSource
{
    public class OrderSourceDto
    {
        public int Id { get; set; }

        public string SourceName { get; set; } = null!;

        public int PercentCommission { get; set; }
    }
}
