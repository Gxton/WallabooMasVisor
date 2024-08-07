using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Wallaboo.Models
{
    public class RegistroViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }=null!;
        [Required]
        [MaxLength(50)]
        public string NombreComercial { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string DireccionComercial { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoComercial { get; set; } = null!;

        [Required]
        //[MaxLength(150)]
        public string DescripcionComercial { get; set; } = null!;

        //public string URLComercial { get; set; }=null!;
    }
}
