using dotnet_boilderplate.SharedKernel.Results;
using dotnet_boilderplate.SharedKernel.Utils;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Commands.Drivers.DeleteDriver
{
    public class DeleteDriverHandler
    {
        private readonly ILogger<DeleteDriverHandler> _logger;
        private readonly VrpDbContext _dbContext;
        private readonly string _loggerPrefix = Utils.GetLoggerPrefix<DeleteDriverHandler>();

        public DeleteDriverHandler(ILogger<DeleteDriverHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<DeleteDriverResponse>> Handle(Guid driverId)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                _logger.LogError($"{_loggerPrefix} Driver not found with ID: {driverId}");
                return Result.Failure<DeleteDriverResponse>(Error.NotFound($"Driver with ID {driverId} not found"));
            }

            _dbContext.Drivers.Remove(driver);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"{_loggerPrefix} Driver deleted successfully: {driverId}");

            return Result.Success<DeleteDriverResponse>(new DeleteDriverResponse(driverId, true));
        }
    }
}
