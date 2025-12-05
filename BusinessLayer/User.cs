using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    
     public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }

        private User()
        {
            Tasks = new List<TaskItem>();
        }

        public User(string username, string email, string passwordHash)
        {

            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Tasks = new List<TaskItem>();
        }
    }
}

