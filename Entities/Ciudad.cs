using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallaboo.Interfaces;

namespace Wallaboo.Entities
{
    public class Ciudad:IEntidadComun
    {
        public int Id {  get; set; }
        public string NombreCiudad { get; set; } = null!;
        public int ProvinciaId {  get; set; }
        public int PaisId {  get; set; }
        //public Provincia Provincia { get; set; } = null!;
    }
}
