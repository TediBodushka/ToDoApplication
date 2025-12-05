using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var vm = new DashboardViewModel
            {
                Categories = _context.Categories.ToList(),
                Tasks = _context.Tasks.ToList(),
                SelectedCategoryId = categoryId
            };

            return View(vm);
        }

        public IActionResult Calendar(DateTime? date)
        {
            var current = date ?? DateTime.Today;

            var vm = new CalendarViewModel
            {
                CurrentMonth = new DateTime(current.Year, current.Month, 1),
                Tasks = _context.Tasks.ToList()
            };

            return View(vm);
        }

        public IActionResult Profile()
        {
            var vm = new ProfileViewModel
            {
                TotalTasks = _context.Tasks.Count(),
                FinishedTasks = _context.Tasks.Count(t => t.IsCompleted),
                NotFinishedTasks = _context.Tasks.Count(t => !t.IsCompleted),
                Categories = _context.Categories.Count()
            };

            return View(vm);
        }

    
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
                Color = string.IsNullOrEmpty(color) ? "#1E90FF" : color
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
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
