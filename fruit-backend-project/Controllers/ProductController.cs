using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using fruit_backend_project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _productService.GetByIdAsync((int)id);
            List<Product> products = await _productService.GetAllAsync();
            List<Category> categories = await _categoryService.GetAllCategoriesWithProductCount();

            if (product == null)
            {
                return NotFound();
            }

            return View(new ShopVM() { Product = product, Categories = categories, Products = products });
        }
    }
}
