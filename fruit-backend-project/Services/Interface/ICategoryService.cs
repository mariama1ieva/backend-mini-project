using fruit_backend_project.Areas.ViewModels.Categories;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace fruit_backend_project.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetALlCategories();
        Task<SelectList> GetALlBySelectedAsync();
        Task<List<Category>> GetAllCategoriesWithProductCount();



    }
}
