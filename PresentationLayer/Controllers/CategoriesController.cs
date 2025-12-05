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

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string color)
        {
            if (string.IsNullOrWhiteSpace(title))
                return View();

            var category = new Category
            {
                Title = title,
                Color = string.IsNullOrEmpty(color) ? "#1E90FF" : color
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
