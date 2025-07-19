namespace JoinBackendDotnet.DTOs
{
    public class ContactResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonPropertyName("bg_color")]
        public string BgColor { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonPropertyName("user")]
        public int UserId { get; set; }
    }
}
