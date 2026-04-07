using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Administrator.Properties
{
    public class PropertiesViewModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PropertyCount { get; set; }
    }
}
