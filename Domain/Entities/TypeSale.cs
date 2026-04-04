using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class TypeSale
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // Corregido de Nombre a Name
        public string Description { get; set; } // Corregido de Descripcion a Description
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
