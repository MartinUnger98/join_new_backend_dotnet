using JoinBackendDotnet.Models;

namespace JoinBackendDotnet.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Category Category { get; set; }
        public Status Status { get; set; }

        public List<Contact> AssignedTo { get; set; } = new();

        public List<Subtask> Subtasks { get; set; } = new();
    }

}
