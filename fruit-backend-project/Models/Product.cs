using System.ComponentModel.DataAnnotations.Schema;

namespace fruit_backend_project.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Origin { get; set; }
        public string Quality { get; set; }
        public string Сheck { get; set; }

        public int Rating { get; set; } = 0;

        [Column(TypeName = "decimal(18, 6)")]
        public decimal MinWeight { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
