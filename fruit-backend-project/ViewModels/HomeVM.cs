using fruit_backend_project.Models;

namespace fruit_backend_project.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderInfo SliderInfo { get; set; }

        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Feature> Features { get; set; }

        public List<ServiceContent> ServiceContents { get; set; }
        public List<FactContent> FactContents { get; set; }
        public Fresh Freshes { get; set; }




    }
}
