using fruit_backend_project.Areas.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace fruit_backend_project.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetALlCategories();
        Task<SelectList> GetALlBySelectedAsync();

    }
}
