namespace ApplicationCore.ModelsDto.Product
{
    public class ComboDto
    {
        public Guid Id { get; set; }

        public string ProductNumber { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public int WholesalePrice { get; set; }

        public string Image { get; set; } = null!;

        public int OnHand { get; set; }

        public bool IsActive { get; set; }

        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductTypeName { get; set; } = null!;

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public List<ProductDto> products { get; set; } = new List<ProductDto>();
    }
}
