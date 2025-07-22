namespace JoinBackendDotnet.DTOs
{
    public class SubtaskDto
    {
        public int Id { get; set; }
        public int Task { get; set; }
        public string? Value { get; set; }
        public bool Edit { get; set; }
        public bool Done { get; set; }
    }
}
