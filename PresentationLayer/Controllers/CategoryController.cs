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

        // LIST not needed – categories се показват в Home > Index

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
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

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // EDIT POST
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

        // DELETE GET (confirmation)
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // DELETE POST (with delete tasks inside)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            // delete all tasks belonging to this category
            var tasks = _context.Tasks.Where(t => t.CategoryId == id).ToList();
            _context.Tasks.RemoveRange(tasks);

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
