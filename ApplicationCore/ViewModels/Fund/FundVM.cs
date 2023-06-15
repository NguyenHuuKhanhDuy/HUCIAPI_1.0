namespace ApplicationCore.ViewModels.Fund
{
    public class FundVM
    {
        public string Name { get; set; }

        public int TotalFund { get; set; }

        public Guid UserCreateId { get; set; }

        public Guid UserAssignId { get; set; }

        public string Note { get; set; }
    }
}
