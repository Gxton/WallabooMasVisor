using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Wallaboo.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PagoViewModel
    {
        public int AnuncioId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        [DisplayName("Costo")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [DisplayName("Dias a pagar")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es obligatorio.")]
        [CreditCard(ErrorMessage = "El número de tarjeta no es válido.")]
        [DisplayName("Número de tarjeta")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "El mes de expiración es obligatorio.")]
        [Range(1, 12, ErrorMessage = "El mes de expiración debe estar entre 1 y 12.")]
        [DisplayName("Mes expiracion")]
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessage = "El año de expiración es obligatorio.")]
        [Range(0, 99, ErrorMessage = "El año de expiración debe ser válido.")]
        [DisplayName("Año expiracion")]
        public int ExpirationYear { get; set; }

        [Required(ErrorMessage = "El nombre del titular de la tarjeta es obligatorio.")]
        [DisplayName("Titular")]
        public string CardholderName { get; set; }

        [Required(ErrorMessage = "El código de seguridad es obligatorio.")]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "El código de seguridad debe tener 3 dígitos.")]
        [DisplayName("Código de seguridad")]
        public string SecurityCode { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [DisplayName("Email Mercado Pago")]
        public string Email { get; set; }
    }
}
