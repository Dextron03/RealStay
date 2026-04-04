using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class PropertyType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
