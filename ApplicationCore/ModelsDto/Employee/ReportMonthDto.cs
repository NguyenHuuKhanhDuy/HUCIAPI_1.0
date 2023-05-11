namespace ApplicationCore.ModelsDto.Employee
{
    public class ReportMonthDto
    {
        public int TotalOrder { get; set; }
        public BenefitDto Benefit { get; set; } = new BenefitDto();
        public int MonthAgo { get; set; }
    }
}
