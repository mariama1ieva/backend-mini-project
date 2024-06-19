using fruit_backend_project.Data;
using Microsoft.AspNetCore.Mvc;

namespace fruit_backend_project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
