namespace ApplicationCore.ViewModels.CallTakeCare
{
    public class CallTakeCareVM
    {
        public Guid OrderId { get; set; }

        public Guid UserCreateId { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
