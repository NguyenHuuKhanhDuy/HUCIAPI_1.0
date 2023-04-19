namespace ApplicationCore.ViewModels.Fund
{
    public class FundDetailVM
    {
        public Guid FundId { get; set; }

        public int TypeFundId { get; set; }

        public string TypeFundName { get; set; } = null!;

        public int AmountMoney { get; set; }

        public Guid UserCreateId { get; set; }

        public string Note { get; set; } = null!;
    }
}
