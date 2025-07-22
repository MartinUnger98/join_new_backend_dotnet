using System.ComponentModel.DataAnnotations;

namespace JoinBackendDotnet.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public BgColor BgColor { get; set; } = BgColor.Color1;

        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Task> Tasks { get; set; } = new();
    }

    public enum BgColor
    {
        Color1, // #FF7A00
        Color2, // #462F8A
        Color3, // #FFBB2B
        Color4, // #FC71FF
        Color5, // #6E52FF
        Color6, // #1FD7C1
        Color7, // #9327FF
        Color8  // #FF4646
    }
}
