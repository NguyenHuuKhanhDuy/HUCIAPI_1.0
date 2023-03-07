namespace ApplicationCore.ModelsDto.Brand
{
    public class BrandDto
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public Guid UserCreateId { get; set; }
        
        public string UserCreateName { get; set; } = null!;
    }
}
