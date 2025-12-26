using dotnet_boilderplate.SharedKernel.Results;
using dotnet_template.AuthService.Domains;
using dotnet_template.AuthService.Persistence;
using dotnet_template.AuthService.Persistence.Utils;
using Microsoft.EntityFrameworkCore;

namespace dotnet_template.AuthService.Features.Commands.LoginWithPassword
{
    public class LoginWithPasswordHandler
    {
        private readonly AuthDbContext _dbContext;

        private ILogger<LoginWithPasswordHandler> _logger;

        public LoginWithPasswordHandler(ILogger<LoginWithPasswordHandler> logger, AuthDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<Result<LoginWithPasswordResponse>> Handle(LoginWithPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return Result.Failure<LoginWithPasswordResponse>(new Error("Login.Invalid", "User not found"));
            }

            if (!PasswordUtils.ComparedHash(request.Password, user.PasswordHash))
            {
                return Result.Failure<LoginWithPasswordResponse>(new Error("Login.Invalid", "Password is wrong"));
            }

            var token = JwtGenerator.GenerateToken(user);

            return Result.Success<LoginWithPasswordResponse>(new LoginWithPasswordResponse(token));

        }
    }
}
