using dotnet_boilderplate.SharedKernel.Results;
using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Persistence;
using dotnet_template.AuthService.Projections;
using Microsoft.EntityFrameworkCore;

namespace dotnet_template.AuthService.Features.Commands.GetUsers
{
    public class GetUsersHandler
    {
        private readonly AuthDbContext _dbContext;

        private readonly ILogger<GetUsersHandler> _logger;

        public GetUsersHandler(ILogger<GetUsersHandler> logger, AuthDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<GetUsersResponse>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Users.AsNoTracking();

            var total = await query.CountAsync();

            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                query = query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(request.PageSize.Value * request.PageNumber.Value)
                    .Take(request.PageSize.Value);
            }

            var data = await query.ToListAsync();

            var transformData = data.Select(u => new LiteUserViewModel(u.Id, u.Email, u.Roles)).ToList();

            var result = new ResponseList<LiteUserViewModel>(
                request.PageSize.HasValue ? request.PageSize.Value : 1, 
                request.PageNumber.HasValue ? request.PageNumber.Value : total, 
                total,
                transformData);

            return Result.Success(new GetUsersResponse(result));
        }
    }
}
