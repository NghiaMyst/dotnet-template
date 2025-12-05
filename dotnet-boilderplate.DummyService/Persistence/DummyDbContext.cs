using Microsoft.EntityFrameworkCore;

namespace dotnet_boilderplate.DummyService.Persistence
{
    public class DummyDbContext : DbContext
    {
        public DummyDbContext(DbContextOptions options) : base(options)
        {
        }

        protected DummyDbContext()
        {
        }
    }
}
