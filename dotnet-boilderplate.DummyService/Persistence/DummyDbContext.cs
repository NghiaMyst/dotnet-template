using dotnet_boilderplate.DummyService.Domains.Aggregates;
using dotnet_boilderplate.DummyService.Domains.Entities;
using dotnet_boilderplate.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;

namespace dotnet_boilderplate.DummyService.Persistence
{
    public class DummyDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>(); 

        public DummyDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DummyDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var currentUser = "current-user";

            // TODO: need monitoring 
            foreach (var entry in ChangeTracker.Entries<BaseEntity<EntityId>>())
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
