using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Login
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Nombre de usuario incorrecto")]
        public required string UserName { get; set; }

        [Required(ErrorMessage ="Contraseña incorrecta")]
        public required string Password { get; set; }
    }
}
