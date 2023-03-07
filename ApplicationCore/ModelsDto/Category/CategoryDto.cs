namespace ApplicationCore.ModelsDto.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public Guid UserCreateId { get; set; }
    }
}
