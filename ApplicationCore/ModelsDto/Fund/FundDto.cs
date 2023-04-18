namespace ApplicationCore.ModelsDto.Fund
{
    public class FundDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalFund { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = string.Empty;

        public string Note { get; set; }

        public List<FundDetailDto> FundDetails { get; set; } = new List<FundDetailDto>();
    }
}
