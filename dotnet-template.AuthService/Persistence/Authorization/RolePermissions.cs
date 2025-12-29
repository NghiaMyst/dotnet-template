using dotnet_template.AuthService.Domains.Aggregates;

namespace dotnet_template.AuthService.Persistence.Authorization
{
    public static class RolePermissions
    {
        public static IReadOnlyList<string> GetPermissionsByRole(RoleTypes role)
        {
            return role switch 
            { 
                RoleTypes.Admin => new List<string>()
                {
                    Permissions.Users_View,
                    Permissions.Users_Create,
                    Permissions.Users_Delete,
                    Permissions.Users_Update
                },
                RoleTypes.Moderator => new List<string>(),
                RoleTypes.User => new List<string>(),
                _ => new List<string>()
            };
        }

        public static List<string> GetAllPermissions(IEnumerable<string> roles)
        {
            var permissions = new HashSet<string>();
            foreach (var roleStr in roles)
            {
                if (Enum.TryParse<RoleTypes>(roleStr, out var role))
                {
                    permissions.UnionWith(GetPermissionsByRole(role));
                }
            }
            return [.. permissions];
        }
    }

    public static class Permissions
    {
        public const string Users_View = "Users_View";
        public const string Users_Create = "Users_Create";
        public const string Users_Update = "Users_Update";
        public const string Users_Delete = "Users_Delete";
    }

    public static class Policies
    {
        public const string CanViewUsers = nameof(CanViewUsers);
        public const string CanCreateUsers = nameof(CanCreateUsers);
        public const string CanUpdateUsers = nameof(CanUpdateUsers);
        public const string CanDeleteUsers = nameof(CanDeleteUsers);
        public const string CanManageUsers = nameof(CanManageUsers);
    }
}
