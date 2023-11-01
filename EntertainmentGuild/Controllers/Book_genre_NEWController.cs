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
    public class Book_genre_NEWController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Book_genre_NEWController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
              return _context.Book_genre_NEW != null ? 
                          View(await _context.Book_genre_NEW.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Book_genre_NEW'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subGenreID,Name")] Book_genre_NEW book_genre_NEW)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book_genre_NEW);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book_genre_NEW);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book_genre_NEW == null)
            {
                return NotFound();
            }

            var book_genre_NEW = await _context.Book_genre_NEW.FindAsync(id);
            if (book_genre_NEW == null)
            {
                return NotFound();
            }
            return View(book_genre_NEW);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("subGenreID,Name")] Book_genre_NEW book_genre_NEW)
        {
            if (id != book_genre_NEW.subGenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book_genre_NEW);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Book_genre_NEWExists(book_genre_NEW.subGenreID))
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
            return View(book_genre_NEW);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book_genre_NEW == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book_genre_NEW'  is null.");
            }
            var book_genre_NEW = await _context.Book_genre_NEW.FindAsync(id);
            if (book_genre_NEW != null)
            {
                _context.Book_genre_NEW.Remove(book_genre_NEW);
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

        private bool Book_genre_NEWExists(int id)
        {
          return (_context.Book_genre_NEW?.Any(e => e.subGenreID == id)).GetValueOrDefault();
        }
    }
}
