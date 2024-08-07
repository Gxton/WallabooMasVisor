using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallabo.Entities;
using Wallaboo.Interfaces;


namespace Wallaboo.Entities
{
    public class Anuncio:IEntidadTenant
    {
        public int Id {  get; set; }
        public string Descripcion { get; set; } = null!;
        public string TenantId { get; set; } = null!;
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta {  get; set; }
        public decimal Precio { get; set; }
        public int CantidadDias {  get; set; }
        public int Activo { get; set; } 
        public int Pagado {  get; set; }
    }
}
