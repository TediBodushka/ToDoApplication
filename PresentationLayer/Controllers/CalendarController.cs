using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

public class CalendarController : Controller
{
    private readonly ToDoListDbContext _taskContext;

    public CalendarController(ToDoListDbContext taskContext)
    {
        _taskContext = taskContext;
    }

    public IActionResult Index(DateTime? date)
    {
        var selected = date ?? DateTime.Today; // change

        var firstDay = new DateTime(selected.Year, selected.Month, 1);
        var daysInMonth = DateTime.DaysInMonth(selected.Year, selected.Month);

        var vm = new CalendarViewModel
        {
            CurrentMonth = firstDay,
            SelectedDate = selected,     // 👈 IMPORTANT
            CalendarDays = Enumerable.Range(0, daysInMonth)
                                     .Select(i => firstDay.AddDays(i))
                                     .ToList(),
            Tasks = _taskContext.Tasks
                .Include(t => t.Category)
                .ToList()
        };

        return View(vm);
    }

}

