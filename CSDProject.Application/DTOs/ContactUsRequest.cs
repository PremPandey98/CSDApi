namespace CSDProject.Application.DTOs
{
    public class ContactUsRequest
    {
        public int ContactId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? MblNumber { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
