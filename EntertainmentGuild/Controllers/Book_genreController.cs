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
    public class Book_genreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Book_genreController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
              return _context.Book_genre != null ? 
                          View(await _context.Book_genre.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Book_genre'  is null.");
        }

       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subGenreID,Name")] Book_genre book_genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book_genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book_genre);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book_genre == null)
            {
                return NotFound();
            }

            var book_genre = await _context.Book_genre.FindAsync(id);
            if (book_genre == null)
            {
                return NotFound();
            }
            return View(book_genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("subGenreID,Name")] Book_genre book_genre)
        {
            if (id != book_genre.subGenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book_genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Book_genreExists(book_genre.subGenreID))
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
            return View(book_genre);
        }

       
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book_genre == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book_genre'  is null.");
            }
            var book_genre = await _context.Book_genre.FindAsync(id);
            if (book_genre != null)
            {
                _context.Book_genre.Remove(book_genre);
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

        private bool Book_genreExists(int id)
        {
          return (_context.Book_genre?.Any(e => e.subGenreID == id)).GetValueOrDefault();
        }
    }
}
