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
    public class StocktakesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocktakesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stocktakes.Include(s => s.Product).Include(s => s.Source);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name");
            ViewData["SourceId"] = new SelectList(_context.Sources, "sourceid", "Source_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,SourceId,ProductId,Quantity,Price")] Stocktake stocktake)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stocktake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", stocktake.ProductId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "sourceid", "Source_name", stocktake.SourceId);
            return View(stocktake);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stocktakes == null)
            {
                return NotFound();
            }

            var stocktake = await _context.Stocktakes.FindAsync(id);
            if (stocktake == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", stocktake.ProductId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "sourceid", "Source_name", stocktake.SourceId);
            return View(stocktake);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,SourceId,ProductId,Quantity,Price")] Stocktake stocktake)
        {
            if (id != stocktake.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stocktake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StocktakeExists(stocktake.ItemId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name", stocktake.ProductId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "sourceid", "Source_name", stocktake.SourceId);
            return View(stocktake);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stocktakes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Stocktakes'  is null.");
            }
            var stocktake = await _context.Stocktakes.FindAsync(id);
            if (stocktake != null)
            {
                _context.Stocktakes.Remove(stocktake);
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

        private bool StocktakeExists(int id)
        {
          return (_context.Stocktakes?.Any(e => e.ItemId == id)).GetValueOrDefault();
        }
    }
}
