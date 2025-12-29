using dotnet_template.AuthService.Domains.Aggregates;
using dotnet_template.AuthService.Persistence.Authorization;

namespace dotnet_template.AuthService.Persistence.Extensions
{
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Register Role, Claim, Policy
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddCustomAuthorization(this WebApplicationBuilder builder)
        {
            #region Users Perms
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy(Policies.CanViewUsers, policy => policy.RequireClaim("Permissions", Permissions.Users_View));

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy(Policies.CanManageUsers, 
                    policy => policy.RequireClaim("Permissions", 
                        Permissions.Users_View, 
                        Permissions.Users_Update,
                        Permissions.Users_Delete,
                        Permissions.Users_Create));
            #endregion

            return builder;
        }
    }
}
