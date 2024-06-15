namespace fruit_backend_project.Areas.ViewModels.Products
{
    public class ProductDetailVM
    {
        public string Category { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public decimal MinWeight { get; set; }
        public decimal Weight { get; set; }
        public string Origin { get; set; }
        public string Quality { get; set; }
        public string Check { get; set; }
        public List<ProductImageVM> Images { get; set; }
    }
}
