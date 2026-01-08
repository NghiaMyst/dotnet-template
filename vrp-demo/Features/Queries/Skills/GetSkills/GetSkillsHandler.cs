using dotnet_boilderplate.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;
using vrp_demo.Domains.ValueObjects;
using vrp_demo.Persistence;

namespace vrp_demo.Features.Queries.Skills.GetSkills
{
    public class GetSkillsHandler
    {
        private readonly ILogger<GetSkillsHandler> _logger;
        private readonly VrpDbContext _dbContext;

        public GetSkillsHandler(ILogger<GetSkillsHandler> logger, VrpDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<ResponseList<Skill>>> Handle(int? pageNumber, int? pageSize)
        {
            var query = _dbContext.Skills.AsNoTracking();

            var total = await query.CountAsync();

            if (pageNumber != null && pageSize != null)
            {
                query = query
                    .Skip(pageSize.Value * (pageNumber.Value - 1))
                    .Take(pageSize.Value);
            }

            try
            {
                var results = await query.ToListAsync();
                
                return Result.Success<ResponseList<Skill>>(new ResponseList<Skill>(pageSize ?? 0, pageNumber ?? 0, total, results));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"{nameof(GetSkillsHandler)}: {ex.Message}");

                return Result.Failure<ResponseList<Skill>>(Error.Failure($"{ex.Message}"));
            }
        }

    }
}
