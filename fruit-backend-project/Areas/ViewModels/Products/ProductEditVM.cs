namespace fruit_backend_project.Areas.ViewModels.Products
{
    public class ProductEditVM
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public string Category { get; set; }


        public decimal Price { get; set; }

        public decimal MinWeight { get; set; }

        public decimal Weight { get; set; }

        public string Origin { get; set; }

        public string? Quality { get; set; }

        public string Check { get; set; }

        public List<ProductImageVM>? Images { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public int Rating { get; set; }

    }
}
