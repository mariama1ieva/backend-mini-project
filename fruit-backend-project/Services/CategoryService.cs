using fruit_backend_project.Areas.ViewModels.Categories;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using fruit_backend_project.Services.Interface;
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
    }
}
