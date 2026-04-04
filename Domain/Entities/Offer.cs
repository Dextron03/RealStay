using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Offer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal OfferAmount { get; set; }
        public string Status { get; set; } = ""; // Aceptado, Pendiente,Rechazo
        public string UserId { get; set; }
        public string PropertyId {get; set;}
        public Property Property { get; set; }
        public DateTime DateRegistration { get; set; } = DateTime.Now;
    }
}