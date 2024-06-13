using System.ComponentModel.DataAnnotations;

namespace fruit_backend_project.Areas.ViewModels.Categories
{
    public class CategoryCreatVM
    {
        [Required]
        public string Name { get; set; }
    }
}
