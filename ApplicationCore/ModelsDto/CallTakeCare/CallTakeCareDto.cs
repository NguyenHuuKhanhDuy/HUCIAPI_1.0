namespace ApplicationCore.ModelsDto.CallTakeCare
{
    public class CallTakeCareDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string Notes { get; set; } = null!;
    }
}
