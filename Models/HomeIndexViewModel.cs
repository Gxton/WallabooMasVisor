
using System.ComponentModel.DataAnnotations;
using Wallaboo.Entities;

namespace Wallaboo.Models
{
    public class HomeIndexViewModel
    {
        [Required]
        [StringLength(1000)]
        [Display(Name = "Ingresa el contenido de tu anuncio")]
        public string Descripcion { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Activo desde:")]

        public DateOnly FechaDesde { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Activo hasta:")]
        [DataType(DataType.Date)]

        public DateOnly FechaHasta { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
        [Display(Name = "Ingresa el precio de lo que anuncias")]
        public decimal Precio { get; set; }
        public int CantidadDias {  get; set; }
        public string Activo { get; set; } 
        public IEnumerable<Anuncio> Anuncios { get; set; } = new List<Anuncio>();
        public IEnumerable<Pais> Paises { get; set; } = new List<Pais>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaDesde.Day <= FechaHasta.Day)
            {
                yield return new ValidationResult("La fecha de finalizacion no puede ser anterior a la de inicio", new[] { "EndDate" });
            }
        }

    }
}
