using fruit_backend_project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public ContactViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(await _settingService.GetAll()));
        }
    }
}
