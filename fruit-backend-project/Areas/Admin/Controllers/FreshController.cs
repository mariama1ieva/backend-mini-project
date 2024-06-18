using fruit_backend_project.Areas.ViewModels.Fresh;
using fruit_backend_project.Data;
using fruit_backend_project.Helpers.Extentions;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FreshController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FreshController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Fresh> fresh = await _context.Freshes.ToListAsync();
            return View(fresh);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FreshCreateVM create)
        {
            if (!ModelState.IsValid) return View();

            if (!create.Image.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!create.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 200KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await create.Image.SaveFileToLocalAsync(path);
            await _context.Freshes.AddAsync(new Fresh
            {
                Image = fileName,
                Title = create.Title,
                SubTitle = create.SubTitle,
                PriceFirst = create.PriceFirst,
                PriceSecond = create.PriceSecond,
                Description = create.Description
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Fresh fresh = await _context.Freshes.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (fresh == null) return NotFound();
            FreshDetailVM model = new()
            {
                Id = fresh.Id,
                Title = fresh.Title,
                Image = fresh.Image,
                SubTitle = fresh.SubTitle,
                PriceSecond = fresh.PriceSecond,
                PriceFirst = fresh.PriceFirst,
                Description = fresh.Description,

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Fresh fresh = await _context.Freshes.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (fresh is null) return NotFound();

            fresh.Image.DeleteFile(_env.WebRootPath, "img");

            _context.Freshes.Remove(fresh);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return BadRequest();
            Fresh fresh = await _context.Freshes.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (fresh == null) return NotFound();

            return View(new FreshEditVM
            {
                Image = fresh.Image,
                Id = fresh.Id,
                Title = fresh.Title,
                SubTitle = fresh.SubTitle,
                PriceSecond = fresh.PriceSecond,
                PriceFirst = fresh.PriceFirst,
                Description = fresh.Description,
            });


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, FreshEditVM request)
        {
            Fresh fresh = await _context.Freshes.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                request.Image = fresh.Image;
                return View(request);
            }

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be 200kb");
                    return View(request.Photo);
                }

                if (!request.Photo.CheckFileFormat("image/"))
                {
                    ModelState.AddModelError("Photo", "Image format is wrong");
                    return View(request.Photo);
                }
                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), fresh.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                fresh.Image = fileName;
            }

            if (fresh == null) { return NotFound(); }

            fresh.Title = request.Title;
            fresh.Description = request.Description;
            fresh.SubTitle = request.SubTitle;
            fresh.PriceFirst = request.PriceFirst;
            fresh.PriceSecond = request.PriceSecond;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
