using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PropertyImprovement
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<PropertyImprovements> PropertyImprovements { get; set; } = new HashSet<PropertyImprovements>();
    }
}
