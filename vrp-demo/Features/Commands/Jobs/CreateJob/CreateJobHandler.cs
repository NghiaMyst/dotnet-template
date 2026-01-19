using dotnet_boilderplate.SharedKernel.Results;
using dotnet_boilderplate.SharedKernel.Utils;
using vrp_demo.Domains.Aggregates;
using vrp_demo.Domains.Enums;
using vrp_demo.Features.Commands.Drivers.CreateDriver;
using vrp_demo.Persistence;
using vrp_demo.Persistence.Services;

namespace vrp_demo.Features.Commands.Jobs.CreateJob
{
    public class CreateJobHandler
    {
        private readonly ILogger<CreateJobHandler> _logger;
        private readonly VrpDbContext _dbContext;
        private readonly string _loggerPrefix = Utils.GetLoggerPrefix<CreateDriverHandler>();
        private readonly JobCodeGenerator _codeGenerator;

        public CreateJobHandler(VrpDbContext dbContext, ILogger<CreateJobHandler> logger, JobCodeGenerator codeGenerator)
        {
            _dbContext = dbContext;
            _logger = logger;
            _codeGenerator = codeGenerator;
        }

        public async Task<Result<CreateJobResponse>> Handle(string? description, JobType jobType, IEnumerable<CreateTaskRequest> tasks, CancellationToken ct)
        {
            var newCode = await _codeGenerator.GenerateAsync(jobType, ct);

            var job = Job.CreateJob(newCode, jobType, description ?? string.Empty, tasks);

            if (job.IsFailure)
            {
                return Result.Failure<CreateJobResponse>(job.Error);
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.Jobs.AddAsync(job.Value, ct);
                await _dbContext.Tasks.AddRangeAsync(job.Value.Tasks, ct);
                await _dbContext.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }

            return Result.Success<CreateJobResponse>(new CreateJobResponse(job.Value));
        }
    }
}
