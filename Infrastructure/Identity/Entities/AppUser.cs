using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PathImg { get; set; } 
        public string IdentityNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    
        // Navigation Properties
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();
        public ICollection<WishList> WishLists { get; set; } = new HashSet<WishList>();
        public ICollection<Offer> Offers { get; set; } = new HashSet<Offer>();
        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
    }
}
