using System.ComponentModel.DataAnnotations;

namespace fruit_backend_project.Areas.ViewModels.ServiceContent
{
    public class ServiceCreateVM
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
