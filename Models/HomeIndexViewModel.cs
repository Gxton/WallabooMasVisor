
using Wallaboo.Entities;

namespace Wallaboo.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Anuncio> Anuncios { get; set; }=new List<Anuncio>();
       // public IEnumerable<Pais> Paises {  get; set; } = new List<Pais>();
    }
}
