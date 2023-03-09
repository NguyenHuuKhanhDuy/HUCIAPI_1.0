namespace ApplicationCore.ViewModels.Product
{
    public class NameRelationOfProduct
    {
        public string BrandName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string UserCreateName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;

        public NameRelationOfProduct(string brandName, string categoryName, string userCreateName, string productTypeName)
        {
            BrandName = brandName;
            CategoryName = categoryName;
            UserCreateName = userCreateName;
            ProductTypeName = productTypeName;
        }
    }
}
