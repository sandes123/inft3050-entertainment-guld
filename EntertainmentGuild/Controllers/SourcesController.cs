using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntertainmentGuild.Data;
using EntertainmentGuild.Models;

namespace EntertainmentGuild.Controllers
{
    public class SourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sources.Include(s => s.Genre1);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("sourceid,Source_name,externalLink,Genre")] Source source)
        {
            if (ModelState.IsValid)
            {
                _context.Add(source);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", source.Genre);
            return View(source);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sources == null)
            {
                return NotFound();
            }

            var source = await _context.Sources.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", source.Genre);
            return View(source);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("sourceid,Source_name,externalLink,Genre")] Source source)
        {
            if (id != source.sourceid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(source);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceExists(source.sourceid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", source.Genre);
            return View(source);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sources == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sources'  is null.");
            }
            var source = await _context.Sources.FindAsync(id);
            if (source != null)
            {
                _context.Sources.Remove(source);
            }

            int check = _context.SaveChanges();
            if (check == 1)
            {
                return new JsonResult("Delete successfully");
            }

            else
            {
                return new JsonResult("Error!");
            }
        }

        private bool SourceExists(int id)
        {
          return (_context.Sources?.Any(e => e.sourceid == id)).GetValueOrDefault();
        }
    }
}
