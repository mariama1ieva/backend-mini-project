using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _productService.GetByIdAsync((int)id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}
