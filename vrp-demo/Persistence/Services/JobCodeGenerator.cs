using Microsoft.EntityFrameworkCore;
using Npgsql;
using vrp_demo.Domains.Enums;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Persistence.Services
{
    public class JobCodeGenerator
    {
        private readonly ILogger<JobCodeGenerator> _logger;

        private readonly VrpDbContext _context;

        public JobCodeGenerator(VrpDbContext context, ILogger<JobCodeGenerator> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<JobCode> GenerateAsync(JobType jobType, CancellationToken cancellationToken)
        {
            var prefix = GetPrefix(jobType);
            var sequence = await GetNextSequenceAsync(prefix, cancellationToken);

            return JobCode.Create($"{prefix}-{sequence}");
        }

        private string GetPrefix(JobType jobType) => jobType switch
        {
            JobType.Shipment => "SH",
            JobType.Service => "SE",
            _ => "JOB"
        };

        private async Task<int> GetNextSequenceAsync(string prefix, CancellationToken cancellationToken)
        {
            //var count = await _context.Jobs.Where(j => j.Code.Code.StartsWith(prefix)).CountAsync(cancellationToken);

            //var count = await _context.Jobs
            //    .Where(j => EF.Property<string>(j, "code").StartsWith(prefix))
            //    .CountAsync(cancellationToken);

            var count = await _context.Jobs
                .FromSqlRaw(
                    @"SELECT * FROM job WHERE code LIKE @prefix || '%'",
                    new NpgsqlParameter("prefix", prefix)
                )
                .CountAsync(cancellationToken);

            return count + 1;
        }
    }
}
