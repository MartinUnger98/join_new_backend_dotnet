using Microsoft.EntityFrameworkCore;
using JoinBackendDotnet.Models;

namespace JoinBackendDotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<JoinBackendDotnet.Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AuthToken> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enum als String
            modelBuilder.Entity<Contact>()
                .Property(c => c.BgColor)
                .HasConversion<string>();

            // Email-Index
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
