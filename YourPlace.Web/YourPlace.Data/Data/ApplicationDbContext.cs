
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using YourPlace.Models.Models;

namespace YourPlace.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookedHour> BookedHours { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Raiting> Raitings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreService> StoreServices { get; set; }
        public DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
