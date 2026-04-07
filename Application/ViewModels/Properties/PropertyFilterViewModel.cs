using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Properties
{
    public class PropertyFilterViewModel
    {
        public string? TypeSaleName { get; set; }
        public int? Rooms {get; set;}
        public int? Bathrooms {get; set;}
    }
}