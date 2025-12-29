using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Domains.ValueObjects;

namespace dotnet_template.AuthService.Projections
{
    public record LiteUserViewModel(UserId Id, string Email, List<string> Roles);
}
