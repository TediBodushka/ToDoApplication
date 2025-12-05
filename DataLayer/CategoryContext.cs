using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public class CategoryContext : IDb<Category, int>
    {
        private readonly ToDoListDbContext dbContext;

        public CategoryContext(ToDoListDbContext context)
        {
            dbContext = context;
        }

        public void Create(Category item)
        {
            if (string.IsNullOrEmpty(item.Color))
            {
                item.Color = "#1E90FF";
            }

            dbContext.Categories.Add(item);
            dbContext.SaveChanges();
        }

        public Category Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.Tasks);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            Category category = query.FirstOrDefault(c => c.Id == key);

            if (category == null)
            {
                throw new KeyNotFoundException();
            }

            return category;
        }

        public List<Category> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Category> query = dbContext.Categories;

            if (useNavigationalProperties)
            {
                query = query.Include(c => c.Tasks);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return query.ToList();
        }

        public void Update(Category item, bool useNavigationalProperties = false)
        {
            Category existing = Read(item.Id, useNavigationalProperties);

            existing.Title = item.Title;
            existing.Color = item.Color;   

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Category category = Read(key, useNavigationalProperties: true);
            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();
        }
    }
}
