namespace ApplicationCore.ModelsDto.IP
{
    public class IPDto
    {
        public Guid Id { get; set; }

        public string Ipv4 { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string Notes { get; set; } = null!;
    }
}
