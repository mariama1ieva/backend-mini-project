using fruit_backend_project.Areas.ViewModels.SliderInfos;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {

        private readonly AppDbContext _context;
        public SliderInfoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            SliderInfo sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();
            return View(sliderInfo);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfos.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (sliderInfo == null) return NotFound();
            SliderInfoDetailVM model = new()
            {
                Title = sliderInfo.Title,
                SubTitle = sliderInfo.SubTitle,


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
        public async Task<IActionResult> Create(SliderInfoCreateVM info)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existInfo = await _context.SliderInfos.AnyAsync(m => m.Title == info.Title
                                                           && m.SubTitle == info.SubTitle);
            if (existInfo)
            {
                ModelState.AddModelError("Title", "These inputs already exist");
            }

            await _context.SliderInfos.AddAsync(new SliderInfo
            {
                Title = info.Title,
                SubTitle = info.SubTitle


            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfos.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (sliderInfo == null) return NotFound();

            _context.SliderInfos.Remove(sliderInfo);
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
            SliderInfo sliderInfo = await _context.SliderInfos.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (sliderInfo == null) return NotFound();

            return View(new SliderInfoEditVM
            {
                Title = sliderInfo.Title,
                SubTitle = sliderInfo.SubTitle
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderInfoEditVM edit, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            SliderInfo slider = await _context.SliderInfos.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (edit == null) return NotFound();

            slider.Title = edit.Title;
            slider.SubTitle = edit.SubTitle;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
