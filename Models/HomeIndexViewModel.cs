
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

        [DataType(DataType.DateTime)]
        [Display(Name = "Activo desde:")]
        public DateTime FechaDesde { get; set; } = DateTime.Now;

        [Display(Name = "Activo hasta:")]
        [DataType(DataType.DateTime)]
        public DateTime FechaHasta { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Precio")]
        [Range(0.01, 9999.99)]
        public decimal Precio { get; set; }

        public int CantidadDias { get; set; }
        public int Activo { get; set; }
        public int Pagado { get; set; }
        public IEnumerable<Anuncio> Anuncios { get; set; } = new List<Anuncio>();

        // Propiedad para las imágenes subidas
        [Display(Name = "Seleccionar Imágenes")]
        public IEnumerable<IFormFile>? Imagenes { get; set; }

        // Nueva propiedad para las imágenes ya guardadas
        public IEnumerable<Imagen>? ImagenesGuardadas { get; set; }

        public HomeIndexViewModel()
        {
            ImagenesGuardadas = new List<Imagen>(); // Inicializa la lista para evitar null.
        }

        // Otras propiedades...
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaDesde > FechaHasta)
            {
                yield return new ValidationResult("La fecha de finalización no puede ser anterior a la de inicio", new[] { "FechaHasta" });
            }
        }
    }
}


