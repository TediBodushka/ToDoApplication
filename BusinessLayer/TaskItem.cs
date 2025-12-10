using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey (nameof(CategoryId))]
        public virtual Category? Category { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public TaskItem()
        {

        }
        public TaskItem(string title, string description, DateTime? dueDate, int categoryId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            CategoryId = categoryId;
          
        }
        public TaskItem(string title,string description, DateTime? dueDate, bool isComplated, int categoryId, int userId)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            IsCompleted= isComplated;
            CategoryId = categoryId;
            UserId = userId;
        }

        public TaskItem(string title, string description, DateTime? dueDate, int categoryId, int v) : this(title, description, dueDate, categoryId)
        {
        }
    }
}
