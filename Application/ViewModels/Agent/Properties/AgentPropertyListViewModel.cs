using System.Collections.Generic;

namespace Application.ViewModels.Agent.Properties
{
    public class AgentPropertyListViewModel
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
        public string FirstImage { get; set; } = string.Empty;
        public bool IsSold => Status == "Sold" || Status == "Vendida";
    }
}