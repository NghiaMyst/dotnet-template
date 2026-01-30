using dotnet_boilderplate.SharedKernel.Results;
using dotnet_boilderplate.SharedKernel.Utils;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Commands.Drivers.AddSkillToDriver
{
    public class AddSkillToDriverHandler
    {
        private readonly ILogger<AddSkillToDriverHandler> _logger;
        private readonly VrpDbContext _dbContext;
        private readonly string _loggerPrefix = Utils.GetLoggerPrefix<AddSkillToDriverHandler>();

        public AddSkillToDriverHandler(ILogger<AddSkillToDriverHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<AddSkillToDriverResponse>> Handle(Guid driverId, List<Guid> skillIds)
        {
            var driver = await _dbContext.Drivers.FirstOrDefaultAsync(d => d.Id == driverId);

            if (driver == null)
            {
                _logger.LogError($"{_loggerPrefix} Driver not found with ID: {driverId}");
                return Result.Failure<AddSkillToDriverResponse>(Error.NotFound($"Driver with ID {driverId} not found"));
            }

            var skillsExist = await _dbContext.Skills
                .Where(s => skillIds.Contains(s.Id))
                .Select(s => s.Id)
                .ToListAsync();

            if (skillsExist.Count != skillIds.Count)
            {
                var missingSkills = skillIds.Except(skillsExist).ToList();
                _logger.LogError($"{_loggerPrefix} Skills not found: {string.Join(", ", missingSkills)}");
                return Result.Failure<AddSkillToDriverResponse>(Error.NotFound($"Skills not found: {string.Join(", ", missingSkills)}"));
            }

            var addResult = driver.AddSkills(skillIds);

            if (addResult.IsFailure)
                return Result.Failure<AddSkillToDriverResponse>(addResult.Error);

            _dbContext.Drivers.Update(driver);
            await _dbContext.SaveChangesAsync();

            return Result.Success<AddSkillToDriverResponse>(new AddSkillToDriverResponse(driver.Id, driver.SkillIds));
        }
    }
}
