using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Administrator.Sales
{
    public class EditTypeSaleDto
    {
        public required string TypeName { get; set; }
        public required string Description { get; set; }
    }
}
