using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagement2.Models;
using InventoryManagement2.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace InventoryManagement2.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult AddReview([FromForm] Review review)
        {
            var product = _context.Products.Find(review.ProductId);
            if (product == null)
            {
                return BadRequest("Invalid product ID.");
            }

            review.Product = product;
            _context.Reviews.Add(review);
            _context.SaveChanges();

            var reviews = _context.Reviews
                .Where(r => r.ProductId == review.ProductId)
                .OrderByDescending(r => r.CreatedDate)
                .ToList();

            return PartialView("_ReviewList", reviews);
        }

        
        public async Task<IActionResult> UploadImage(int productId, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }

                
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                
                product.ImagePath = $"/images/{fileName}";
                _context.Update(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = productId });
        }
    }
}
