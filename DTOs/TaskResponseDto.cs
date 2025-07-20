namespace JoinBackendDotnet.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? DueDate { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public string? AssignedTo { get; set; }
        public bool Done { get; set; }
    }
}
