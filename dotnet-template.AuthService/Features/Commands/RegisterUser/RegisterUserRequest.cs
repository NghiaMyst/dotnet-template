namespace dotnet_template.AuthService.Features.Commands.RegisterUser
{
    public record RegisterUserRequest(string Email, string Password, string ConfirmPassword);

    public record RegisterUserResponse(string Email);
}
