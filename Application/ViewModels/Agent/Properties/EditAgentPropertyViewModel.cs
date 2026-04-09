using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Agent.Properties
{
    public class EditAgentPropertyViewModel
    {
        [Required(ErrorMessage = "El tipo de propiedad es requerido")]
        public string PropertyTypeId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de venta es requerido")]
        public string TypeSaleId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tamaño es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El tamaño debe ser mayor a 0")]
        public int Meters { get; set; }

        [Required(ErrorMessage = "La cantidad de habitaciones es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de habitaciones no puede ser negativa")]
        public int NumberRooms { get; set; }

        [Required(ErrorMessage = "La cantidad de baños es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de baños no puede ser negativa")]
        public int NumberBaths { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        public string Location { get; set; } = string.Empty;

        public List<string> ImprovementIds { get; set; } = new();

        public List<IFormFile>? Images { get; set; }

        public List<string>? ExistingImages { get; set; }
    }
}