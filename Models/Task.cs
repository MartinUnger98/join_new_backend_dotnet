using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoinBackendDotnet.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public string Priority { get; set; } = "Urgent";

        [Required]
        public string Category { get; set; } = "Technical Task";

        [Required]
        public string Status { get; set; } = "To do";

        public List<int> AssignedTo { get; set; } = new();

        public List<Subtask> Subtasks { get; set; } = new();
    }
}
