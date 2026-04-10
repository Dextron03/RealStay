namespace Application.DTOs.Agent
{
    public class AgentProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? PathImg { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}