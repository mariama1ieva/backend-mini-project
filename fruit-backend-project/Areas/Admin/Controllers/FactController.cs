﻿using fruit_backend_project.Areas.ViewModels.FactContent;
using fruit_backend_project.Data;
using fruit_backend_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fruit_backend_project.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class FactController : Controller
    {
        private readonly AppDbContext _context;
        public FactController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<FactContent> factContent = await _context.FactContents.ToListAsync();
            return View(factContent);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            FactContent factContent = await _context.FactContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (factContent == null) return NotFound();
            FactDetailVM model = new()
            {
                Title = factContent.Title,
                NumberInfo = factContent.NumberInfo,
                Icon = factContent.Icon

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
        public async Task<IActionResult> Create(FactCreateVM fact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existfact = await _context.FactContents.AnyAsync(m => m.Title == fact.Title
                                                           && m.Icon == fact.Icon
                                                           && m.NumberInfo == fact.NumberInfo);
            if (existfact)
            {
                ModelState.AddModelError("Title", "These inputs already exist");
            }

            await _context.FactContents.AddAsync(new FactContent
            {
                Title = fact.Title,
                NumberInfo = fact.NumberInfo,
                Icon = fact.Icon

            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            FactContent factContent = await _context.FactContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (factContent == null) return NotFound();

            _context.FactContents.Remove(factContent);
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
            FactContent factContent = await _context.FactContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (factContent == null) return NotFound();

            return View(new FactEditVM
            {
                Title = factContent.Title,
                Icon = factContent.Icon,
                NumberInfo = factContent.NumberInfo
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FactEditVM edit, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            FactContent factContent = await _context.FactContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (edit == null) return NotFound();

            factContent.Title = edit.Title;
            factContent.Icon = edit.Icon;
            factContent.NumberInfo = edit.NumberInfo;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
