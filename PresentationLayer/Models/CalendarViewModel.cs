namespace PresentationLayer.Models
{
    using System;
    using System.Collections.Generic;
    using BusinessLayer;

    public class CalendarViewModel
    {
        public DateTime CurrentMonth { get; set; }
        public DateTime SelectedDate { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<TaskItem> DayTasks { get; set; } = new List<TaskItem>();

        public List<DateTime> CalendarDays { get; set; } = new List<DateTime>();
    }
}
