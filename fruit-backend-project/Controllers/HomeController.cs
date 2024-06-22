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
        private readonly ICategoryService categoryService;
        public HomeController(AppDbContext context, IProductService productService, ICategoryService categoryService)
        {
            _context = context;
            this.productService = productService;
            this.categoryService = categoryService;
        }


        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.Include(m => m.Products).Where(m => !m.SoftDelete).ToListAsync();
            List<Product> products = await productService.GetAllAsync();
            List<Feature> features = await _context.Features.ToListAsync();
            List<ServiceContent> serviceContents = await _context.ServiceContents.ToListAsync();
            List<FactContent> factContents = await _context.FactContents.ToListAsync();
            Fresh freshes = await _context.Freshes.FirstOrDefaultAsync();
            List<Review> reviews = await _context.Reviews.Include(m => m.AppUser).ToListAsync();


            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfo = sliderInfo,
                Categories = categories,
                Products = products,
                Features = features,
                ServiceContents = serviceContents,
                FactContents = factContents,
                Freshes = freshes,
                Reviews = reviews,
            };

            return View(model);
        }
    }
}
