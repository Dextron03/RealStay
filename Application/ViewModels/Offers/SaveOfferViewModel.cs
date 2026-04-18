using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Offers
{
    public class SaveOfferViewModel
    {
        [Required, Range(1, double.MaxValue)]
        public decimal OfferAmount { get; set; }
        public string? UserId { get; set; }
        [Required]
        public string PropertyId {get; set;}
    }
}