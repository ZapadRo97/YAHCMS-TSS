
using Microsoft.EntityFrameworkCore;
using YAHCMS.CulturalService.Configuration;
using YAHCMS.CulturalService.Models;

namespace YAHCMS.CulturalService.Persistence
{
    public class CulturalDbContext : DbContext
    {
        public CulturalDbContext(DbContextOptions<CulturalDbContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ApplyConfiguration(new ArtistTypeConfiguration());
        }

        //sets
        public DbSet<ArtistType> artistTypes {get; set;}
        public DbSet<Artist> artists {get; set;}
        public DbSet<Location> locations {get; set;}
        public DbSet<Quiz> quizes {get; set;}
        public DbSet<QuizQuestion> questions {get; set;}

    }
}