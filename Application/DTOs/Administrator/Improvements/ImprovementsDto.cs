using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Administrator.Improvements
{
    public class ImprovementsDto
    {
        public string Id { get; set; }
        public string ImprovementName { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
    }
}
