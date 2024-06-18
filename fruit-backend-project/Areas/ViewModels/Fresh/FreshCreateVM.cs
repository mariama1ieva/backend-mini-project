using System.ComponentModel.DataAnnotations;

namespace fruit_backend_project.Areas.ViewModels.Fresh
{
    public class FreshCreateVM
    {

        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Title { get; set; }
        [Required]
        public string SubTitle { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int PriceFirst { get; set; }
        [Required]
        public int PriceSecond { get; set; }

        [Required]
        public IFormFile? Image { get; set; }
    }
}
