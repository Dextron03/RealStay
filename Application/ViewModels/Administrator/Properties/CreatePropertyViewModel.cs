using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Administrator.Properties
{
    public class CreatePropertyViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string ImprovementName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Description { get; set; } = string.Empty;
    }
}
