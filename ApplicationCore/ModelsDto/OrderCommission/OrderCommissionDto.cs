namespace ApplicationCore.ModelsDto.OrderCommission
{
    public class OrderCommissionDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid EmployeeId { get; set; }

        public int OrderTotal { get; set; }

        public int OrderCommission1 { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
