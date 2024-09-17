using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wallaboo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; }
    }
}
