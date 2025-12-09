using System.Collections.Generic;
using BusinessLayer;


    namespace PresentationLayer.Models
    {
        public class DashboardViewModel
        {
            public List<Category> Categories { get; set; } = new();
            public List<TaskItem> Tasks { get; set; } = new();
            public int? SelectedCategoryId { get; set; }
            public DateTime SelectedDate { get; set; }

    }
}


