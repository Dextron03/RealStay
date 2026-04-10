using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Agent
{
    public class UpdateAgentProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}