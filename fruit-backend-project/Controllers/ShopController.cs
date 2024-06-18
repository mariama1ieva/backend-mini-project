﻿using fruit_backend_project.Data;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using fruit_backend_project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ShopController(AppDbContext context, ICategoryService categoryService, IProductService productService)
        {
            _context = context;
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productService.GetAllAsync();
            List<Category> categories = await _categoryService.GetAllCategoriesWithProductCount();


            ShopVM model = new()
            {
                Products = products,
                Categories = categories
            };
            return View(model);
        }

        public async Task<IActionResult> Sorting(string sort)
        {
            IEnumerable<Product> products = await _productService.GetAllAsync();

            switch (sort)
            {
                case "A to Z":
                    products = products.OrderBy(m => m.Name);
                    break;
                case "Z to A":
                    products = products.OrderByDescending(m => m.Name);
                    break;
                case "Old to New":
                    products = products.OrderBy(m => m.Id);
                    break;
                case "Cheap to Expensive":
                    products = products.OrderBy(m => m.Price);
                    break;
                case "Expensive to Cheap":
                    products = products.OrderByDescending(m => m.Price);
                    break;
            }

            ShopVM model = new() { Products = products };

            return PartialView("_ProductsFilterPartial", model);
        }
    }
}