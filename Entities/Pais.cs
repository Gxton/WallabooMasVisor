using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallaboo.Interfaces;


namespace Wallaboo.Entities
{
    public class Pais:IEntidadComun
    {
        public int id {  get; set; }
        public string NombrePais { get; set; } = null!;
       // public List<Provincia> Provincias { get; set; } = null!;

    }
}
