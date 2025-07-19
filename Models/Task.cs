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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        // Platzhalter: Liste von Namen als Strings (später als Contact-Objekte ersetzen)
        public List<string> AssignedTo { get; set; } = new List<string>();
    }

    public enum Priority
    {
        Urgent,
        Medium,
        Low
    }

    public enum Category
    {
        TechnicalTask,
        UserStory,
        Bug
    }

    public enum Status
    {
        ToDo,
        InProgress,
        AwaitFeedback,
        Done
    }
}
