using DataLayer;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

public class CalendarController : Controller
{
    private readonly ToDoListDbContext _taskContext;

    public CalendarController(ToDoListDbContext taskContext)
    {
        _taskContext = taskContext;
    }

    public IActionResult Index()
    {
        var current = DateTime.Now;

        var vm = new CalendarViewModel
        {
            CurrentMonth = new DateTime(current.Year, current.Month, 1),
            Tasks = _taskContext.Tasks.ToList()
        };

        return View(vm);
    }
}
