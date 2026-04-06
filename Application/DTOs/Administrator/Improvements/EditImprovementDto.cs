using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Administrator.Improvements
{
    public class EditImprovementDto
    {
        public required string ImprovementName { get; set; }
        public required string Description { get; set; }
    }
}
