using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YAHCMS.UserService.Models;

namespace YAHCMS.UserService.Persistence 
{
    public class UserRepository : IUserRepository
    {
        private UserDbContext context;

        public UserRepository(UserDbContext context)
        {
            this.context = context;
        }
        public User Add(User user)
        {
            var b = context.Add(user);
            context.SaveChanges();
            return b.Entity;
        }

        public User Delete(string userID)
        {
            User u = this.Get(userID);
            context.Remove(u);
            return u;
        }

        public User Get(string userID)
        {
            return context.users.FirstOrDefault(u => u.UID == userID);
        }

        public IEnumerable<User> GetAll()
        {
            return context.users.ToList();
        }

        public User Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return user;
        }
    }
}