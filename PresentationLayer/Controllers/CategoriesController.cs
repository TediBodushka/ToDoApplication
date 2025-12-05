using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using DataLayer;

namespace PresentationLayer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ToDoListDbContext _context;

        public CategoryController(ToDoListDbContext context)
        {
            _context = context;
        }

        // ===============================
        // DISPLAY ALL CATEGORIES
        // ===============================
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // ===============================
        // CREATE (GET)
        // ===============================
        public IActionResult Create()
        {
            return View();
        }

        // ===============================
        // CREATE (POST)
        // ===============================
        [HttpPost]
        public IActionResult Create(string title, string color)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("title", "Title is required");
                return View();
            }

            var category = new Category
            {
                Title = title,
                Color = color // <- store selected hex color
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ===============================
        // EDIT (GET)
        // ===============================
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ===============================
        // EDIT (POST)
        // ===============================
        [HttpPost]
        public IActionResult Edit(int id, string title, string color)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            category.Title = title;
            category.Color = color; // update selected color

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ===============================
        // DELETE
        // ===============================
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
