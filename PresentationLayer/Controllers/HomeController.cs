using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;
using System;
using System.Linq;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListDbContext _context;

        public HomeController(ToDoListDbContext context)
        {
            _context = context;
        }

        // ---------------- DASHBOARD ----------------
        public IActionResult Index(int? categoryId)
        {
            var vm = new DashboardViewModel
            {
                Categories = _context.Categories.ToList(),
                Tasks = _context.Tasks.ToList(),
                SelectedCategoryId = categoryId
            };

            return View(vm);
        }


        // ---------------- TASK CRUD ----------------

        public IActionResult CreateTask()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult CreateTask(TaskListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // DEBUG
                Console.WriteLine("ModelState Errors: " + string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)));

                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                CategoryId = model.CategoryId,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult EditTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(task);
        }

        [HttpPost]
        public IActionResult EditTask(TaskItem model)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == model.Id);
            if (task == null) return NotFound();

            task.Title = model.Title;
            task.Description = model.Description;
            task.DueDate = model.DueDate;
            task.CategoryId = model.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // ---------------- CATEGORY CRUD ----------------

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(string title, string color)
        {
            if (string.IsNullOrWhiteSpace(title))
                return View();

            var category = new Category
            {
                Title = title,
                Color = color
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(int id, string title, string color)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            category.Title = title;
            category.Color = color;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            // remove tasks in category
            var tasks = _context.Tasks.Where(t => t.CategoryId == id).ToList();
            _context.Tasks.RemoveRange(tasks);

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // ---------------- DETAILS ----------------

        public IActionResult TaskDetails(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            return View(task);
        }
    }
}
