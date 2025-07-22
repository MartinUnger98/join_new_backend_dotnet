using System.Text.Json.Serialization;

namespace JoinBackendDotnet.Models
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginDto
    {
        [JsonPropertyName("username_or_email")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
} 