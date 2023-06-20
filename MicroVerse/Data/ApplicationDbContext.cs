using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Models;
using MicroVerse.ViewModels;

namespace MicroVerse.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext() : base() {}

        public DbSet<Post> Post { get; set; }

        public DbSet<Follows> Follows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.PostId, v.UserId });
            modelBuilder.Entity<Follows>()
                .HasKey(f => new { f.FollowingUserId, f.FollowedUserId });
        }
    }
}
