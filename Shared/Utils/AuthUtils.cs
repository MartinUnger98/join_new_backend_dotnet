using System.Security.Cryptography;
using System.Text;

namespace JoinBackendDotnet.Shared.Utils
{
    public static class AuthUtils
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
}