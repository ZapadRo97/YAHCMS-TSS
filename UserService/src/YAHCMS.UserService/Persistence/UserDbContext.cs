using Microsoft.EntityFrameworkCore;
using YAHCMS.UserService.Models;

namespace YAHCMS.UserService.Persistence 
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) :
            base(options)
        {
        }

        public DbSet<User> users {get; set;}
    }
}