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
    public class ProductController : Controller
    {
        private readonly ApplicationDataContext _context;

        public ProductController(ApplicationDataContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> ProductList()
        {
            var applicationDataContext = _context.Products.Include(p => p.Category);
            return View(await applicationDataContext.ToListAsync());
        }

        
        public async Task<IActionResult> ProductDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

       
        public IActionResult AddProduct()
        {
            ViewBag.CategoryList = new SelectList(_context.Category, "Id", "Description");
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> AddProduct([Bind("Id,Name,Description,Price,ImageUrl,CategoryId")] Product product, IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + image.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","Product", fileName); 
            using (var stream = new FileStream(filePath,FileMode.Create))
            {
                await image.CopyToAsync(stream);    
            }
            product.ImageUrl = Path.Combine("images", "Product", fileName);
                product.Id = Guid.NewGuid();
                _context.Add(product);
                await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ProductList));

        }


        public async Task<IActionResult> EditProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", product.CategoryId);
            return View(product);
        }

        
        [HttpPost]
       
        public async Task<IActionResult> EditProduct(Guid id, [Bind("Id,Name,Description,Price,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

           
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductList));
          
        }

        
        public async Task<IActionResult> DeleteProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost]
        
        public async Task<IActionResult> DeleteProductConfirmate(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
