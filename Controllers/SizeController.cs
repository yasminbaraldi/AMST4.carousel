using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMST4.CAROUSEL.Context;
using AMST4.CAROUSEL.Models;

namespace AMST4.CAROUSEL.Controllers
{
    public class SizeController : Controller
    {
        private readonly ApplicationDataContext _context;

        public SizeController(ApplicationDataContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> SizeList()
        {
            return View(await _context.Size.ToListAsync());

        }

        [HttpGet]
        public IActionResult AddSize()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> AddSize([Bind("Id,Description,IsActive,CreatedAt")] Size size)
        {
            
                size.Id = Guid.NewGuid();
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        
        public async Task<IActionResult> SizeEdit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> SizeEdit(Guid id, [Bind("Id,Description,IsActive,CreatedAt")] Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.Id))
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
            return View(size);
        }

        
        public async Task<IActionResult> SizeDelete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size
                .FirstOrDefaultAsync(m => m.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var size = await _context.Size.FindAsync(id);
            if (size != null)
            {
                _context.Size.Remove(size);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(Guid id)
        {
            return _context.Size.Any(e => e.Id == id);
        }
    }
}
