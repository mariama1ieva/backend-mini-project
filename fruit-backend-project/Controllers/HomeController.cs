using fruit_backend_project.Data;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using fruit_backend_project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService productService;
        public HomeController(AppDbContext context, IProductService productService)
        {
            _context = context;
            this.productService = productService;
        }


        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.Include(m => m.Products).Where(m => !m.SoftDelete).ToListAsync();
            List<Product> products = await productService.GetAllAsync();


            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Categories = categories,
                Products = products
            };

            return View(model);
        }
    }
}
