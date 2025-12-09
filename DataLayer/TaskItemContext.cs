using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class TaskItemContext : IDb<TaskItem, int>
    {
        private readonly ToDoListDbContext dbContext;

        public TaskItemContext(ToDoListDbContext context)
        {
            dbContext = context;
        }

        public void Create(TaskItem item)
        {
            dbContext.Tasks.Add(item);
            dbContext.SaveChanges();
        }

        public TaskItem Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<TaskItem> query = dbContext.Tasks;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(t => t.Category)
                    .Include(t => t.User);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            TaskItem taskItem = query.FirstOrDefault(t => t.Id == key);

            if (taskItem == null)
            {
                throw new KeyNotFoundException();
            }

            return taskItem;
        }

        public List<TaskItem> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<TaskItem> query = dbContext.Tasks;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(t => t.Category)
                    .Include(t => t.User);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return query.ToList();
        }

        public void Update(TaskItem item, bool useNavigationalProperties = false)
        {
            TaskItem existing = Read(item.Id, useNavigationalProperties);

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.DueDate = item.DueDate;
            existing.IsCompleted = item.IsCompleted;
            existing.CategoryId = item.CategoryId;
            existing.UserId = item.UserId;

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            TaskItem taskItem = Read(key);
            dbContext.Tasks.Remove(taskItem);
            dbContext.SaveChanges();
        }

        public dynamic ReadAllCategories()
        {
            throw new NotImplementedException();
        }
    }
}
