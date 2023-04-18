namespace ApplicationCore.ModelsDto.Fund
{
    public class FundDetailDto
    {
        public Guid Id { get; set; }

        public Guid FundId { get; set; }

        public int TypeFundId { get; set; }

        public string TypeFundName { get; set; } = null!;

        public int AmountMoney { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public string Note { get; set; } = null!;
    }
}
