namespace JoinBackendDotnet.DTOs
{
    public class TaskCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? DueDate { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public string? AssignedTo { get; set; }
    }
}
