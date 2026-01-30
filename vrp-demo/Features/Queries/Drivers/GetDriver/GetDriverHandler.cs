using dotnet_boilderplate.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Domains.Aggregates;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Queries.Drivers.GetDriver
{
    public class GetDriverHandler
    {
        private readonly ILogger<GetDriverHandler> _logger;
        private readonly VrpDbContext _dbContext;

        public GetDriverHandler(ILogger<GetDriverHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<GetDriverResponse>> Handle(Guid driverId)
        {
            var driver = await _dbContext.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                _logger.LogInformation($"{nameof(GetDriverHandler)}: Driver not found with ID: {driverId}");
                return Result.Failure<GetDriverResponse>(Error.NotFound($"Driver with ID {driverId} not found"));
            }

            var response = new GetDriverResponse(
                driver.Id,
                driver.Name,
                driver.Address,
                driver.Location.Y,  // Latitude
                driver.Location.X,  // Longitude
                driver.Role,
                driver.SkillIds
            );

            return Result.Success(response);
        }
    }
}
