using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoinBackendDotnet.DTOs
{
    public class TaskCreateDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("due_date")]
        public string? DueDate { get; set; }

        [Required]
        public string? Priority { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        [JsonPropertyName("assigned_to")]
        public List<int> AssignedTo { get; set; } = new();

        public List<SubtaskCreateTaskDto> Subtasks { get; set; } = new();
    }
}
