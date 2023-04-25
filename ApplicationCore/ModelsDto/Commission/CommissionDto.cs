namespace ApplicationCore.ModelsDto.Commission
{
    public class CommissionDto
    {
        public Guid Id { get; set; }

        public int TotalPriceFrom { get; set; }

        public int CommissionPrice { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public DateTime CreateDate { get; set; }
    }
}
