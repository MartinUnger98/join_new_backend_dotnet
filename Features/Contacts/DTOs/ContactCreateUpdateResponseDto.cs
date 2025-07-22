namespace JoinBackendDotnet.DTOs
{
    public class ContactCreateUpdateResponseDto
    {
        public string Model { get; set; } = "join.contact";
        public int Pk { get; set; }
        public ContactCreateUpdateFields Fields { get; set; } = new();
    }

    public class ContactCreateUpdateFields
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Bg_Color { get; set; } = "";
        public int User { get; set; }
    }
}
