using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Models;

namespace MicroVerse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext() : base() {}

        public DbSet<UserModel> UserModel { get; set; }

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
