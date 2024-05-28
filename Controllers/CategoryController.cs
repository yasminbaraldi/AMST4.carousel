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
    public class CategoryController : Controller
    {
        private readonly ApplicationDataContext _context;

        public CategoryController(ApplicationDataContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> CategoryList()
        {
            return View(await _context.Category.ToListAsync());
        }

       
        public async Task<IActionResult> CategoryDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        
        public IActionResult AddCategory()
        {
            return View();
        }

        
        [HttpPost]
        
        public async Task<IActionResult> AddCategory([Bind("Id,Description,ImageUrl,CreatedAt")] Category category, IFormFile image)

        {
            var fileName = Guid.NewGuid().ToString() + image.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            category.ImageUrl = Path.Combine("images", "Category", fileName);
            category.Id = Guid.NewGuid();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CategoryList));
            }
            
        

        
        public async Task<IActionResult> EditCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        
        public async Task<IActionResult> EditCategory(Guid id,  Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

          
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CategoryList));
            }
            

      
        public async Task<IActionResult> DeleteCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CategoryList));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
