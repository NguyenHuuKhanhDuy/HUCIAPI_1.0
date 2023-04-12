namespace ApplicationCore.ModelsDto.Promotion
{
    public class PromotionDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int QuantityFrom { get; set; }

        public int Price { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;
    }
}
