using ApplicationCore.ModelsDto.OrderCommission;

namespace ApplicationCore.ModelsDto.Employee
{
    public class SalaryEmployeeDto
    {
        public int SalaryEmployee { get; set; }
        public int TotalCommission { get; set; }
        public List<OrderCommissionDto> OrderCommissions { get; set;}
    }
}
