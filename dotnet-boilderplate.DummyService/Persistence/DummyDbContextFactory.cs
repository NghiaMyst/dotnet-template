using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace dotnet_boilderplate.DummyService.Persistence
{
    public class DummyDbContextFactory : IDesignTimeDbContextFactory<DummyDbContext>
    {
        public DummyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DummyDbContext>();

            const string conn =
                "Host=localhost;Port=5432;Database=boilderplate;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(conn);

            return new DummyDbContext(optionsBuilder.Options);
        }
    }
}
