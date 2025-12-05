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

        public IActionResult Index(int? categoryId)
        {
            var categories = _context.Categories.ToList();
            var tasks = _context.Tasks.ToList();

            var vm = new DashboardViewModel
            {
                Categories = categories,
                Tasks = tasks,
                SelectedCategoryId = categoryId
            };

            return View(vm);
        }


        // ====== FIXED CALENDAR ======
        public IActionResult Calendar(DateTime? date)
        {
            var current = date ?? DateTime.Today;

            var vm = new CalendarViewModel
            {
                CurrentMonth = new DateTime(current.Year, current.Month, 1),
                Tasks = _taskContext.Tasks.ToList()
            };

            return View(vm);
        }

        // ====== FIXED PROFILE ======
        public IActionResult Profile()
        {
            return View("Profile");
        }


        // TASK CREATION
        public IActionResult CreateTask()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateTask(TaskListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                CategoryId = model.CategoryId
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult TaskDetails(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            return View(task);
        }
    }
}
