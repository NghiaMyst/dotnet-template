using dotnet_boilderplate.SharedKernel.Results;
using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Persistence;

namespace dotnet_template.AuthService.Features.Commands.RegisterUser
{
    public class RegisterUserHandler
    {
        private readonly AuthDbContext _dbContext;

        private readonly ILogger<RegisterUserHandler> _logger;

        public RegisterUserHandler(ILogger<RegisterUserHandler> logger, AuthDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<RegisterUserResponse>> Handle(RegisterUserRequest request, CancellationToken cancellation)
        {

            var hassPassword = HashPassword(request.Password);

            var userResult = User.Create(request.Email, request.Password);

            if (userResult.IsFailure)
                return Result.Failure<RegisterUserResponse>(userResult.Error);

            var user = userResult.Value;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync(cancellation);

            //var token = JwtGenerator.GenerateToken(user);

            return Result.Success<RegisterUserResponse>(new RegisterUserResponse(user.Email));
        }

        private string HashPassword(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return passwordHash;
        }
    }
}
