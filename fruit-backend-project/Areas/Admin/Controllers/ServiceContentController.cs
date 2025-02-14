﻿using fruit_backend_project.Areas.ViewModels.ServiceContent;
using fruit_backend_project.Areas.ViewModels.ServiceContentVM;
using fruit_backend_project.Data;
using fruit_backend_project.Helpers.Extentions;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceContentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ServiceContentController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ServiceContent> serviceContent = await _context.ServiceContents.ToListAsync();
            return View(serviceContent);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateVM create)
        {
            if (!ModelState.IsValid) return View();

            if (!create.Image.CheckFileFormat("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!create.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await create.Image.SaveFileToLocalAsync(path);
            await _context.ServiceContents.AddAsync(new ServiceContent
            {
                Image = fileName,
                Title = create.Title,
                Description = create.Description
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            ServiceContent serviceContent = await _context.ServiceContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (serviceContent == null) return NotFound();
            ServiceContentDetailVM model = new()
            {
                Id = serviceContent.Id,
                Title = serviceContent.Title,
                Image = serviceContent.Image,
                Description = serviceContent.Description,

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();

            ServiceContent serviceContent = await _context.ServiceContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (serviceContent is null) return NotFound();

            serviceContent.Image.DeleteFile(_env.WebRootPath, "img");

            _context.ServiceContents.Remove(serviceContent);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return BadRequest();
            ServiceContent serviceContent = await _context.ServiceContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (serviceContent == null) return NotFound();

            return View(new ServiceEditVM
            {
                Image = serviceContent.Image,
                Title = serviceContent.Title,
                Description = serviceContent.Description
            });


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ServiceEditVM request)
        {
            ServiceContent serviceContent = await _context.ServiceContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                request.Image = serviceContent.Image;
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
                FileExtention.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), serviceContent.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                serviceContent.Image = fileName;
            }

            if (serviceContent == null) { return NotFound(); }

            serviceContent.Title = request.Title;
            serviceContent.Description = request.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
