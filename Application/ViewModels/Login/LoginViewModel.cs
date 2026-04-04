using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debes colocar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
