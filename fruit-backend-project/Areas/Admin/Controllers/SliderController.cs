using fruit_backend_project.Areas.ViewModels.Sliders;
using fruit_backend_project.Data;
using fruit_backend_project.Helpers.Extentions;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _appDbContext.Sliders.ToListAsync();
            List<SliderVM> model = sliders.Select(m => new SliderVM { Id = m.Id, Name = m.Name, Image = m.Image }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Slider slider = await _appDbContext.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider == null) return NotFound();

            SliderDetailVM model = new()
            {
                Name = slider.Name,
                Image = slider.Image
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
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View();

            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be 200kb");
                return View();
            }

            if (!request.Image.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Image", "Image format is wrong");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
            await request.Image.SaveFileToLocalAsync(path);


            Slider slider = new()
            {
                Name = request.Name,
                Image = fileName
            };

            bool existSlider = await _appDbContext.Sliders.AnyAsync(m => m.Name.Trim() == slider.Name.Trim());
            if (existSlider)
            {
                ModelState.AddModelError("Name", "Slider already exist");
                return View();
            }


            await _appDbContext.Sliders.AddAsync(slider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _appDbContext.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null) return NotFound();

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", slider.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            _appDbContext.Sliders.Remove(slider);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _appDbContext.Sliders.FirstOrDefaultAsync(_ => _.Id == id);
            if (slider == null) return NotFound();

            return View(new SliderEditVM { Id = slider.Id, Name = slider.Name, Image = slider.Image });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderEditVM request, int? id)
        {
            Slider existSlider = await _appDbContext.Sliders.FirstOrDefaultAsync(slider => slider.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existSlider.Image;
                return View(request);
            }
            if (existSlider == null) { return NotFound(); }


            if (!request.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size must be 200kb");
                return View(request);
            }

            if (!request.Photo.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Photo", "Image format is wrong");
                return View(request);
            }



            FileExtention.DeleteFileFromLocalAsync(Path.Combine(_webHostEnvironment.WebRootPath, "img"), existSlider.Image);

            string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);
            await request.Photo.SaveFileToLocalAsync(path);


            existSlider.Name = request.Name;
            existSlider.Image = fileName;


            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));










        }





    }
}
