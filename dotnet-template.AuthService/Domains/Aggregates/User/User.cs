using dotnet_boilderplate.SharedKernel.Common;
using dotnet_boilderplate.SharedKernel.Results;
using dotnet_template.AuthService.Domains.ValueObjects;

namespace dotnet_template.AuthService.Domains.Aggregates.User
{
    public class User : BaseEntity<UserId>
    {
        public string Email { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public string? NormalizedEmail { get; private set;  }

        public List<string> Roles { get; private set; } = [];

        private User() { }

        private User(UserId id, string email, string passwordHash)
        {
            Id = id;
            Email = email;
            NormalizedEmail = email.ToUpperInvariant();
            PasswordHash = passwordHash;
            Roles = [RoleTypes.User.ToString()]; // default
        }

        public static Result<User> Create(string email, string passwordHash)
        {
            if(string.IsNullOrWhiteSpace(email))
                return Result.Failure<User>(new Error("User.EmailRequired", "Email is required"));

            if (string.IsNullOrWhiteSpace(passwordHash))
                return Result.Failure<User>(new Error("User.PasswordRequired", "Password hash is required"));

            var userId = UserId.New();

            return Result.Success(new User(userId, email, passwordHash));
        }

        public void UpdatePassowrdHash(string newPassword)
        {
            PasswordHash = newPassword;
        }

        public void AddRole(RoleTypes role)
        {
            if (!Enum.IsDefined(typeof(RoleTypes), role))
            {
                Roles ??= [];
                Roles.Add(role.ToString());
            }
        }
    }

    public enum RoleTypes 
    { 
        User
    }
}
