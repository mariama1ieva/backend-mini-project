using fruit_backend_project.Models;

namespace fruit_backend_project.ViewModels
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public List<Category> Categories { get; set; }

    }
}
