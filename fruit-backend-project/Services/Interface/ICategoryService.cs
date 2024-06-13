using fruit_backend_project.Areas.ViewModels.Categories;

namespace fruit_backend_project.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetALlCategories();
    }
}
