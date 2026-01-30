using dotnet_boilderplate.SharedKernel.Results;
using dotnet_boilderplate.SharedKernel.Utils;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Domains.Enums;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Commands.Drivers.UpdateDriver
{
    public class UpdateDriverHandler
    {
        private readonly ILogger<UpdateDriverHandler> _logger;
        private readonly VrpDbContext _dbContext;
        private readonly string _loggerPrefix = Utils.GetLoggerPrefix<UpdateDriverHandler>();

        public UpdateDriverHandler(ILogger<UpdateDriverHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<UpdateDriverResponse>> Handle(Guid driverId, string name, string address, double lat, double lng, string roleString)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                _logger.LogError($"{_loggerPrefix} Driver not found with ID: {driverId}");
                return Result.Failure<UpdateDriverResponse>(Error.NotFound($"Driver with ID {driverId} not found"));
            }

            if (!Enum.TryParse<RoleType>(roleString, out var role))
            {
                _logger.LogError($"{_loggerPrefix} Invalid role string: {roleString}");
                return Result.Failure<UpdateDriverResponse>(Error.Validation($"Invalid role string: {roleString}"));
            }

            var updateResult = driver.Update(name, address, lat, lng, role);

            if (updateResult.IsFailure)
                return Result.Failure<UpdateDriverResponse>(updateResult.Error);

            _dbContext.Drivers.Update(driver);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"{_loggerPrefix} Driver updated successfully: {driverId}");

            return Result.Success<UpdateDriverResponse>(new UpdateDriverResponse(driver.Id, driver.Name, driver.Address));
        }
    }
}
