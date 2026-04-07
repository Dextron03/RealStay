using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Administrator.Sales
{
    public class SalesTypeDto
    {
        public int Id { get; set; } 
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PropertyCount { get; set; } 
    }
}
