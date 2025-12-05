using BusinessLayer;
using System;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class CalendarViewModel
    {
        public DateTime CurrentMonth { get; set; }
        public List<TaskItem> Tasks { get; set; }
    }
}
