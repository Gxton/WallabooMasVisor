
using System.ComponentModel.DataAnnotations;
using Wallaboo.Entities;

namespace Wallaboo.Models
{
    public class HomeIndexViewModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(1000)]
        [Display(Name = "Ingresa el contenido de tu anuncio")]
        public string Descripcion { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Activo desde:")]

        public DateTime FechaDesde { get; set; } = DateTime.Now;

        [Display(Name = "Activo hasta:")]
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Ingresa el precio de lo que anuncias")]
        [Range(0.01, 9999.99)]
        public decimal Precio { get; set; }
        public int CantidadDias {  get; set; }
        public int Activo { get; set; } 
        public IEnumerable<Anuncio> Anuncios { get; set; } = new List<Anuncio>();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaDesde.Day <= FechaHasta.Day)
            {
                yield return new ValidationResult("La fecha de finalizacion no puede ser anterior a la de inicio", new[] { "EndDate" });
            }
        }

    }
}
