using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Properties
{
    public class SavePropertyViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string PropertyTypeId { get; set; }

        [Required, Range(1, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int Rooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        public double Meters {get; set; }

        [Required, StringLength(250)]
        public string Description { get; set; }

        public List<int> ImprovementIds { get; set; } = new();
        public List<IFormFile> Images { get; set; } = new();

    }
}