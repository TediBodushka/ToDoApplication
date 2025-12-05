using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public string Name { get; set; } = string.Empty;

        public int CompletedCount { get; set; }

        public int NotCompletedCount { get; set; }

        public List<TaskListViewModel> Tasks { get; set; } = new();
    }
}
