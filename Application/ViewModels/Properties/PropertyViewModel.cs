using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels.Properties
{
    public class PropertyViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string PropertyTypeName { get; set; }
        public string TypeSaleName { get; set; }
        public decimal Price {get; set;}
        public int Rooms {get; set;}
        public int Bathrooms {get; set;}
        public double Meters {get; set;}
        public string Description {get; set;}
        public List<string> ImageUrls {get; set;} = new();
        public List<string> ImprovementNames {get; set;} = new();
        public string AgentId {get; set;}
        public string AgentName {get; set;}
        public string AgentPhone {get; set;}
    }
}