using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Administrator.Improvements
{
    public class CreateImprovementsViewModel
    {
        public required string ImprovementName { get; set; }
        public required string Description { get; set; }
    }
}
