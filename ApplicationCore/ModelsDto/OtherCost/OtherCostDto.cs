namespace ApplicationCore.ModelsDto.OtherCost
{
    public class OtherCostDto
    {
        public Guid Id { get; set; }

        public int Price { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string? Notes { get; set; }
    }
}
