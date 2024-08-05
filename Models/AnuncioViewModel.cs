using System.ComponentModel.DataAnnotations;
using Wallaboo.Entities;

namespace Wallaboo.Models
{
    public class AnuncioViewModel
    {
        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly FechaDesde { get; set; }

        [DataType(DataType.Date)]
        public DateOnly FechaHasta { get; set; }

        [Required]
        public decimal Precio { get; set; }
       // public bool activo { get; set; } = true;
        public IEnumerable<Anuncio> Anuncios { get; set; } = new List<Anuncio>();
    }
}
