using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Properties
{
    public class PropertyFilterViewModel
    {
        public string? TypeSaleName { get; set; }
        public string? PropertyTypeId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Rooms { get; set; }
        public int? Bathrooms { get; set; }
    }
}