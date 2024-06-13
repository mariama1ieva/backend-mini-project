using fruit_backend_project.Data;
using fruit_backend_project.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _appDbContext;
        public SettingService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _appDbContext.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}
