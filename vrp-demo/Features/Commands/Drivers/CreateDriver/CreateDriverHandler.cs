using dotnet_boilderplate.SharedKernel.Results;
using dotnet_boilderplate.SharedKernel.Utils;
using vrp_demo.Domains.Aggregates;
using vrp_demo.Domains.Enums;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Commands.Drivers.CreateDriver
{
    public class CreateDriverHandler
    {
        private ILogger<CreateDriverHandler> _logger;

        private VrpDbContext _dbContext;

        private readonly string _loggerPrefix = Utils.GetLoggerPrefix<CreateDriverHandler>();

        public CreateDriverHandler(ILogger<CreateDriverHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<CreateDriverResponse>> Handle(string name, string address, double lat, double lng, string roleString, List<Guid>? skillIds = null)
        {
            if (!Enum.TryParse<RoleType>(roleString, out var role))
            {
                _logger.LogError($"{_loggerPrefix} Invalid role string: {roleString}");

                return Result.Failure<CreateDriverResponse>(Error.Validation($"[{nameof(CreateDriverHandler)}]: Invalid role string"));
            }

            var driver = Driver.Create(name, address, lat, lng, role, skillIds);

            if (driver.IsFailure)
                return Result.Failure<CreateDriverResponse>(driver.Error);

            await _dbContext.Drivers.AddAsync(driver.Value);
            await _dbContext.SaveChangesAsync();

            return Result.Success<CreateDriverResponse>(new CreateDriverResponse(driver.Value.Id));
        }
    }
}
