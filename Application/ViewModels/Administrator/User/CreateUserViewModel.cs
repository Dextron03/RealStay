using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Administrator.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es requerido")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es requerida")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmar contraseña es requerido")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
