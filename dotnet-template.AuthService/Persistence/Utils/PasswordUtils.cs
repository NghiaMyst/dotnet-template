namespace dotnet_template.AuthService.Persistence.Utils
{
    public static class PasswordUtils
    {
        public static string HashPassword(string origin)
        {
            return BCrypt.Net.BCrypt.HashPassword(origin);
        }

        public static bool ComparedHash(string origin, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(origin, hash);
        }
    }
}
