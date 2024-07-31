using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallaboo.Interfaces;


namespace Wallaboo.Entities
{
    public class Provincia:IEntidadComun
    {
        public int id {  get; set; }
        public string NombreProvincia { get; set; } = null!;
        public int PaisId {  get; set; }
        public Pais Pais { get; set; } = null!;
       // public List<Ciudad> Ciudades { get; set; } = null!;
    }
}
