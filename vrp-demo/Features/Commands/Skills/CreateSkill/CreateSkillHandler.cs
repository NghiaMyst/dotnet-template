using vrp_demo.Persistence;
using dotnet_boilderplate.SharedKernel.Results;
using vrp_demo.Domains.ValueObjects;

namespace vrp_demo.Features.Commands.Skills.CreateSkill
{
    public class CreateSkillHandler
    {
        private ILogger<CreateSkillHandler> _logger;

        private VrpDbContext _dbContext;

        public CreateSkillHandler(VrpDbContext dbContext, ILogger<CreateSkillHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<CreateSkillResponse>> Handle(CreateSkillRequest request, CancellationToken cancellationToken)
        {
            var skill = new Skill(request.Name, request.Description);

            try
            {
                await _dbContext.Skills.AddAsync(skill);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"[{nameof(CreateSkillHandler)}]: Error occur: {ex.Message}");

                return Result.Failure<CreateSkillResponse>(Error.Failure($"{ex.Message}"));
            }

            return Result.Success<CreateSkillResponse>(new CreateSkillResponse(skill.Id));
        }
    }
}
