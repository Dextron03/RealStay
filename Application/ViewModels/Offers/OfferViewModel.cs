using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Offers
{
    public class OfferViewModel
    {

        public string Id { get; set; }
        [Range(1, double.MaxValue)]
        public decimal OfferAmount { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string PropertyId {get; set;}
        public string UserName { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public DateTime DateRegistration { get; set; }
    }
}