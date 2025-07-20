using System.Text.Json.Serialization;

namespace JoinBackendDotnet.Models
{
    public class Subtask
    {
        public int Id { get; set; }

        [JsonIgnore] // wird nur intern benötigt, nicht in Response
        public int Task { get; set; }

        public string Value { get; set; } = string.Empty;

        public bool Edit { get; set; } = false;

        public bool Done { get; set; } = false;
    }
}
