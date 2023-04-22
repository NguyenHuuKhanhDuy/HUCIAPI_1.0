namespace ApplicationCore.ViewModels.Order
{
    public class OrderForLadipageVM
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string Product { get; set; } = null!;
        public string? IpV4 { get; set; }
    }
}
