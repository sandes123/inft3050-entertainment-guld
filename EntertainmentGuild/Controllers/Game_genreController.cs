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
    public class Game_genreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Game_genreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Game_genre != null ? 
                          View(await _context.Game_genre.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Game_genre'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subGenreID,Name")] Game_genre game_genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game_genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game_genre);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Game_genre == null)
            {
                return NotFound();
            }

            var game_genre = await _context.Game_genre.FindAsync(id);
            if (game_genre == null)
            {
                return NotFound();
            }
            return View(game_genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("subGenreID,Name")] Game_genre game_genre)
        {
            if (id != game_genre.subGenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game_genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Game_genreExists(game_genre.subGenreID))
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
            return View(game_genre);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game_genre == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Game_genre'  is null.");
            }
            var game_genre = await _context.Game_genre.FindAsync(id);
            if (game_genre != null)
            {
                _context.Game_genre.Remove(game_genre);
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

        private bool Game_genreExists(int id)
        {
          return (_context.Game_genre?.Any(e => e.subGenreID == id)).GetValueOrDefault();
        }
    }
}
