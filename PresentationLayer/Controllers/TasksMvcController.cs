using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class TasksMvcController : Controller
    {
        private readonly TaskItemContext _taskContext;

        public TasksMvcController(ToDoListDbContext dbContext)
        {
            _taskContext = new TaskItemContext(dbContext);
        }

        [HttpGet]
        public IActionResult CreateTask()
        {
            ViewBag.Categories = _taskContext.ReadAllCategories(); 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(TaskItem task)
        {
            _taskContext.Create(task);
            return RedirectToAction("Index"); 
        }
    }

}



