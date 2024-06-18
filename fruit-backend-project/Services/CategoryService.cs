using fruit_backend_project.Areas.ViewModels.Categories;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryVM>> GetALlCategories()
        {
            List<Category> categories = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();
            return categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name }).ToList();
        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await _context.Categories.ToListAsync();
            return new SelectList(datas, "Id", "Name");
        }

        public async Task<List<Category>> GetAllCategoriesWithProductCount()
        {
            return await _context.Categories.Include(m => m.Products).ToListAsync();


        }
    }
}
