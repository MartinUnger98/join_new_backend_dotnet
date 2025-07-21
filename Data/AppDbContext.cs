using Microsoft.EntityFrameworkCore;
using JoinBackendDotnet.Models;
using ModelsTask = JoinBackendDotnet.Models.Task; // Alias zur Vermeidung von Konflikten mit System.Threading.Tasks.Task

namespace JoinBackendDotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ModelsTask> Tasks { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AuthToken> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🎨 Enum BgColor als string speichern
            modelBuilder.Entity<Contact>()
                .Property(c => c.BgColor)
                .HasConversion<string>();

            // ✉️ Email muss eindeutig sein
            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // 🔗 Task - Subtask Beziehung
            modelBuilder.Entity<Subtask>()
                .HasOne<ModelsTask>()
                .WithMany(t => t.Subtasks)
                .HasForeignKey(s => s.Task)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 Many-to-Many Task <-> Contact (Join-Tabelle TaskContacts)
            modelBuilder.Entity<ModelsTask>()
                .HasMany(t => t.AssignedTo)
                .WithMany(c => c.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "TaskContacts",
                    j => j.HasOne<Contact>().WithMany().HasForeignKey("ContactId"),
                    j => j.HasOne<ModelsTask>().WithMany().HasForeignKey("TaskId")
                );

            // 🔧 Task-Enums als string speichern
            modelBuilder.Entity<ModelsTask>()
                .Property(t => t.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<ModelsTask>()
                .Property(t => t.Category)
                .HasConversion<string>();

            modelBuilder.Entity<ModelsTask>()
                .Property(t => t.Status)
                .HasConversion<string>();
        }
    }
}
