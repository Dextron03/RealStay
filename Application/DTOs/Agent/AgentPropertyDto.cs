using System.Collections.Generic;

namespace Application.DTOs.Agent
{
    public class AgentPropertyDto
    {
        public string Id { get; set; } = string.Empty;
        public string PropertyCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PropertyTypeName { get; set; } = string.Empty;
        public string TypeSaleName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int NumberRooms { get; set; }
        public int NumberBaths { get; set; }
        public int Meters { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string FirstImage { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public List<string> ImprovementNames { get; set; } = new();
        public string PropertyTypeId { get; set; } = string.Empty;
        public string TypeSaleId { get; set; } = string.Empty;
    }
}