using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoListDbContext _context;

        public HomeController(ToDoListDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId = null)
        {
            var tasks = _context.Tasks
                .Include(t => t.Category)
                .ToList<TaskItem>();

            var active = tasks.Where(t => !t.IsCompleted).ToList();
            var completed = tasks.Where(t => t.IsCompleted).ToList();

            if (categoryId.HasValue)
                active = active.Where(t => t.CategoryId == categoryId.Value).ToList();

            var model = new DashboardViewModel
            {
                Tasks = tasks,
                ActiveTasks = active,
                CompletedTasks = completed,
                Categories = _context.Categories.ToList(),
                SelectedCategoryId = categoryId
            };

            return View(model);
        }

        public IActionResult CompleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            task.IsCompleted = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Profile()
        {
            var total = _context.Tasks.Count();
            var completed = _context.Tasks.Count(t => t.IsCompleted);
            var notCompleted = total - completed;

            var vm = new ProfileViewModel
            {
                TotalTasks = total,
                FinishedTasks = completed,
                NotCompletedTasks = notCompleted
            };

            return View(vm);
        }

        public IActionResult Calendar(DateTime? date)
        {
            DateTime selected = date?.Date ?? DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(selected.Year, selected.Month, 1);

            var monthTasks = _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.DueDate.HasValue &&
                            t.DueDate.Value.Month == selected.Month &&
                            t.DueDate.Value.Year == selected.Year)
                .ToList();

            var dayTasks = monthTasks
                .Where(t => t.DueDate.Value.Date == selected.Date)
                .ToList();

            var vm = new CalendarViewModel
            {
                CurrentMonth = firstDayOfMonth,
                SelectedDate = selected,
                Tasks = monthTasks,        // за точките по календара
                DayTasks = dayTasks        // 👈 това добавяме
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
                IsCompleted = false,
                UserId = 1
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
        public IActionResult EditCategory(int id, string title)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            category.Title = title;


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

            var tasks = _context.Tasks.Where(t => t.CategoryId == id).ToList();
            _context.Tasks.RemoveRange(tasks);

            _context.Categories.Remove(category);
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