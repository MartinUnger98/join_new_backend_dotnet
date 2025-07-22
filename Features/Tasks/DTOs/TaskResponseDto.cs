using System.Text.Json.Serialization;

namespace JoinBackendDotnet.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [JsonPropertyName("due_date")]
        public string? DueDate { get; set; }

        public string? Priority { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }

        [JsonPropertyName("assigned_to")]
        public List<int> AssignedTo { get; set; } = new();

        public List<SubtaskDto> Subtasks { get; set; } = new();
    }
}
