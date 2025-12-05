using System;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class CalendarViewModel
    {
        public DateTime CurrentMonth { get; set; }
        public DateTime? SelectedDate { get; set; }
        public List<TaskListViewModel> Tasks { get; set; } = new();
    }
}
