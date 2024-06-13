using System.ComponentModel.DataAnnotations;

namespace fruit_backend_project.Areas.ViewModels.Sliders
{
    public class SliderDetailVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }
    }
}
