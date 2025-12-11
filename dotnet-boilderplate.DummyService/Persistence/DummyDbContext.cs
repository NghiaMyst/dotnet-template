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
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public DummyDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DummyDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var currentUser = "current-user";
            var outboxMessages = new List<OutboxMessage>();

            // TODO: need monitoring 
            foreach (var entry in ChangeTracker.Entries<BaseEntity<EntityId>>())
            {
                if (entry.Entity.DomainEvents.Any())
                {
                    foreach (var domainEvent in entry.Entity.DomainEvents)
                    {
                        var outboxMessage = new OutboxMessage(domainEvent);
                        outboxMessage.SetCreated();
                        outboxMessages.Add(outboxMessage);
                    }

                    entry.Entity.ClearDomainEvents();
                }

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

            OutboxMessages.AddRange(outboxMessages);
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
