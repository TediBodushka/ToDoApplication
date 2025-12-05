using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
   public class UserContext : IDb<User, int>
    {
        private readonly ToDoListDbContext dbContext;

        public UserContext(ToDoListDbContext context)
        {
            dbContext = context;
        }

        public void Create(User item)
        {
            dbContext.Users.Add(item);
            dbContext.SaveChanges();
        }

        public User Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(u => u.Tasks);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            User user = query.FirstOrDefault(u => u.Id == key);

            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            return user;
        }

        public List<User> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<User> query = dbContext.Users;

            if (useNavigationalProperties)
            {
                query = query
                    .Include(u => u.Tasks);
            }

            if (isReadOnly)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            return query.ToList();
        }

        public void Update(User item, bool useNavigationalProperties = false)
        {
            User existing = Read(item.Id, useNavigationalProperties);

            existing.Username = item.Username;
            existing.Email = item.Email;
            existing.PasswordHash = item.PasswordHash;

            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            User user = Read(key, useNavigationalProperties: true);
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }
    }
}