using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities
{
    public class Property
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string PropertyCode { get; set; } = string.Empty;
        public int NumberRooms { get; set; }
        public int NumberBaths { get; set; }
        public int Meters { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = PropertyStatus.Available.ToString();
        
        // Foreign Keys
        public string PropertyTypeId { get; set; } = string.Empty;
        public string TypeSaleId { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;

        // Navigation Properties
        public PropertyType? PropertyType { get; set; }
        public TypeSale? TypeSale { get; set; }

        public ICollection<PropertyImage> Images { get; set; } = new HashSet<PropertyImage>();
        public ICollection<PropertyImprovements> Improvements { get; set; } = new HashSet<PropertyImprovements>();
        public ICollection<Offer> Offers { get; set; } = new HashSet<Offer>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
