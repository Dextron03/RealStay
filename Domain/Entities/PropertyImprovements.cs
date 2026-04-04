using System;

namespace Domain.Entities
{
    public class PropertyImprovements
    {
        public string PropertyId { get; set; } = string.Empty;
        public string ImprovementId { get; set; } = string.Empty;
        
        // Navigation Properties
        public Property? Property { get; set; }
        public PropertyImprovement? Improvement { get; set; }
    }
}
