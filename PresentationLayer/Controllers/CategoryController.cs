using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace PresentationLayer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ToDoListDbContext _context;

        public CategoryController(ToDoListDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string color)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("Title", "Category name is required");
                return View();
            }

            var category = new Category
            {
                Title = title,
                Color = string.IsNullOrEmpty(color) ? "#1E90FF" : color
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, string color)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("Title", "Category title is required");
                return View(category);
            }

            category.Title = title;
            category.Color = color;
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

 
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

       
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

          
            var tasks = _context.Tasks.Where(t => t.CategoryId == id).ToList();
            _context.Tasks.RemoveRange(tasks);

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
