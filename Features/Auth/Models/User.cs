using System.ComponentModel.DataAnnotations;

namespace JoinBackendDotnet.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }

    public class AuthToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
