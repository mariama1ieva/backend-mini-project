using fruit_backend_project.Areas.ViewModels.Products;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _service;
        public ProductController(AppDbContext context, IProductService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {

            List<Product> products = await _service.GetAllAsync();
            List<ProductVM> model = products.Select(m => new ProductVM { Id = m.Id, Name = m.Name, Image = m.ProductImages.FirstOrDefault(m => m.IsMain).Image, MinWeight = m.MinWeight, Weight = m.Weight, Price = m.Price, Origin = m.Origin, Check = m.Сheck, Quality = m.Quality, Description = m.Description, Category = m.Category.Name }).ToList();
            return View(model);

        }
    }
}
