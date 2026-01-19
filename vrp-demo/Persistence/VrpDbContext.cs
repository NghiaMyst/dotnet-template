using dotnet_boilderplate.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Domains.Aggregates;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Persistence
{
    public class VrpDbContext : DbContext
    {
        public VrpDbContext(DbContextOptions options) : base(options)
        {
        }

        protected VrpDbContext()
        {
        }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Domains.Entities.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VrpDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var currentUser = "current-user";

            // TODO: need monitoring 
            foreach (var entry in ChangeTracker.Entries<RootBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreated(currentUser);
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetUpdated(currentUser);
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
