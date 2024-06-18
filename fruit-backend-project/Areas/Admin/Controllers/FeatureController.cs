using fruit_backend_project.Areas.ViewModels.Features;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeatureController : Controller
    {


        private readonly AppDbContext _context;
        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Feature> features = await _context.Features.ToListAsync();
            return View(features);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Feature feature = await _context.Features.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (feature == null) return NotFound();
            FeatureDetailVM model = new()
            {
                Name = feature.Name,
                Icon = feature.Icon,
                Description = feature.Description
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeatureCreateVM feature)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existfeature = await _context.Features.AnyAsync(m => m.Name == feature.Name && m.Icon == feature.Icon && m.Description == feature.Description);
            if (existfeature)
            {
                ModelState.AddModelError("Name", "These inputs already exist");
            }

            await _context.Features.AddAsync(new Feature { Name = feature.Name, Icon = feature.Icon, Description = feature.Description });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Feature feature = await _context.Features.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (feature == null) return NotFound();

            _context.Features.Remove(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            Feature feature = await _context.Features.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (feature == null) return NotFound();

            return View(new FeatureEditVM
            {
                Name = feature.Name,
                Icon = feature.Icon,
                Description = feature.Description
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FeatureEditVM request, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            Feature feature = await _context.Features.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (request == null) return NotFound();

            feature.Description = request.Description;
            feature.Icon = request.Icon;
            feature.Name = request.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}

