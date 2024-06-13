namespace fruit_backend_project.Areas.ViewModels.Sliders
{
    public class SliderEditVM
    {
        public int Id { get; set; }



        public string Name { get; set; }


        public string? Image { get; set; }


        public IFormFile Photo { get; set; }
    }
}
