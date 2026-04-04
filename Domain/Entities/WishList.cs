using System;

namespace Domain.Entities
{
    public class WishList
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateRegistration { get; set; } = DateTime.Now;
        public bool Status { get; set; } = false;
        public string ClientId { get; set; }
        public string PropertyId { get; set; }
        public Property Property { get; set; } // Corregido property a Property
    }
}
