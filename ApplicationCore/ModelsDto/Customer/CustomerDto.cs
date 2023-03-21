namespace ApplicationCore.ModelsDto.Customer
{
    public class CustomerDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime Birthday { get; set; }

        public int Gender { get; set; }

        public int ProvinceId { get; set; }

        public string ProvinceName { get; set; } = null!;

        public int DistrictId { get; set; }

        public string DistrictName { get; set; } = null!;

        public int WardId { get; set; }

        public string WardName { get; set; } = null!;

        public string Notes { get; set; } = null!;

        public int OrderCount { get; set; }

        public Guid CreateUserId { get; set; }

        public string CreateUserName { get; set; } = null!;

        public string IpV4 { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public string Address { get; set; } = null!;
    }
}
