

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using YourPlace.Models.Models;

namespace YourPlace.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookedHour> BookedHours { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Rating> Raitings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreServices> StoreServices { get; set; }
        public DbSet<Town> Towns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Comment>(comment =>
            {
                comment.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BookedHour>(bookedHours =>
            {
                bookedHours.HasOne(b => b.User)
                .WithMany(u => u.BookedHours)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }
    }
}
