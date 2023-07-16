using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroVerse.Models;

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

        public DbSet<Vote> Vote { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // setup composite keys
            modelBuilder.Entity<Vote>()
                .HasKey(v => new { v.PostId, v.UserId });
            modelBuilder.Entity<Follows>()
                .HasKey(f => new { f.FollowingUserId, f.FollowedUserId });
        }
    }
}
