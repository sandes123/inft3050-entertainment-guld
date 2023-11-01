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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Genre1).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name");
            ViewData["LastUpdatedBy"] = new SelectList(_context.Users, "UserName", "UserName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Author,Description,Genre,subGenre,Published,LastUpdatedBy,LastUpdated")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", product.Genre);
            ViewData["LastUpdatedBy"] = new SelectList(_context.Users, "UserName", "UserName", product.LastUpdatedBy);
            return View(product);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", product.Genre);
            ViewData["LastUpdatedBy"] = new SelectList(_context.Users, "UserName", "UserName", product.LastUpdatedBy);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Author,Description,Genre,subGenre,Published,LastUpdatedBy,LastUpdated")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
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
            ViewData["Genre"] = new SelectList(_context.Genres, "genreID", "Name", product.Genre);
            ViewData["LastUpdatedBy"] = new SelectList(_context.Users, "UserName", "UserName", product.LastUpdatedBy);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
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

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
