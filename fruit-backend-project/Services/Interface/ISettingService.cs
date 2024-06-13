namespace fruit_backend_project.Services.Interface
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAll();
    }
}
