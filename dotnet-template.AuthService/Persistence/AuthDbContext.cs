using dotnet_boilderplate.SharedKernel.Common;
using dotnet_template.AuthService.Domains.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace dotnet_template.AuthService.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
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
