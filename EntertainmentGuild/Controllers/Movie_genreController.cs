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
    public class Movie_genreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Movie_genreController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
              return _context.Movie_genre != null ? 
                          View(await _context.Movie_genre.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Movie_genre'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subGenreID,Name")] Movie_genre movie_genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie_genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie_genre);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie_genre == null)
            {
                return NotFound();
            }

            var movie_genre = await _context.Movie_genre.FindAsync(id);
            if (movie_genre == null)
            {
                return NotFound();
            }
            return View(movie_genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("subGenreID,Name")] Movie_genre movie_genre)
        {
            if (id != movie_genre.subGenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie_genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Movie_genreExists(movie_genre.subGenreID))
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
            return View(movie_genre);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie_genre == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie_genre'  is null.");
            }
            var movie_genre = await _context.Movie_genre.FindAsync(id);
            if (movie_genre != null)
            {
                _context.Movie_genre.Remove(movie_genre);
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

        private bool Movie_genreExists(int id)
        {
          return (_context.Movie_genre?.Any(e => e.subGenreID == id)).GetValueOrDefault();
        }
    }
}
