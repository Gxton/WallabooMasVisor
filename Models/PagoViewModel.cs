using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Wallaboo.Models
{
    public class PagoViewModel
    {
        public int Id { get; set; }
        [DisplayName("Costo")]
        public decimal amount { get; set; }

        [Required]
        [DisplayName("Anuncio")]
        public string description { get; set; }


        [Required(ErrorMessage = "Ingrese los 16 numeros de su tarjeta")]
        [DisplayName("Número de tarjeta de crédito")]
        //[CreditCard(ErrorMessage = "La cantidad de numeros de su tarjeta no pueden exeder los 16")]
        [MaxLength(16, ErrorMessage ="Debe ingresar los 16 numeros de su tarjeta de crédito")]
        public string cardNumber {  get; set; }


        [Required(ErrorMessage = "Seleccione el mes de vencimiento de su tarjeta de crédito")]
        [DisplayName("Mes de vencimiento de la tarjeta de crédito")]
        [Range(1, 12, ErrorMessage = "Debe ingresar un mes de vencimiento válido")]
        [MaxLength(2, ErrorMessage ="Debe ingresar solo dos caracteres por el mes")]
        public int expirationMonth { get; set; }


        [Required(ErrorMessage ="Ingrese el año de vencimiento de su tarjeta")]
        [DisplayName("Año de vencimiento de la tarjeta de crédito")]
        [MaxLength(2, ErrorMessage = "Debe ingresar solo dos caracteres por el año")]
        [Range(24, 50, ErrorMessage = "Debe ingresar un año de vencimiento válido")]
        public int expirationYear { get; set; }


        [Required(ErrorMessage ="Debe ingresar su nombre como aparece en su tarjeta")]
        [DisplayName("Nombre completo como figura en la tarjeta")]
        public string cardholderName { get; set; }


        [Required(ErrorMessage ="Debe ingresar el codigo de seguridad")]
        [DisplayName("Código de seguridad")]
        [MaxLength(3)]
        [Range(000,999)]
        public string securityCode { get; set; }


        [Required(ErrorMessage ="Debe ingresar el mail de Mercado Pago")]
        [DisplayName("Email de Mercado Pago")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Formato de emalil incorrecto")]
        public string email {  get; set; }

    }
}
