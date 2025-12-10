using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        
        public string? Color { get; set; }
        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public Category()
        {
            Tasks = new List<TaskItem>();
        }

        public Category(string title)
        {
            Title = title;
            Tasks = new List<TaskItem>();
        }
    }
}
