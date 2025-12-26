namespace dotnet_template.AuthService.Features.Commands.LoginWithPassword
{
    public record LoginWithPasswordRequest(string Email, string Password);

    public record LoginWithPasswordResponse(string Token);
}
