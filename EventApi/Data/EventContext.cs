using EventApi.Model;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Data
{
    public class EventContext :DbContext
    {
        public EventContext(DbContextOptions<EventContext> options)
           : base(options)
        {
        }

        public DbSet<Event> Event { get; set; } = default!;

        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
