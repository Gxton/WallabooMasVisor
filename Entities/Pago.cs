using Wallaboo.Interfaces;

namespace Wallaboo.Entities
{
    public class Pago: IEntidadComun
    {
        public int id {  get; set; }
        public decimal total { get; set; } = 0;
        public int AnuncioID {  get; set; }
        public string TenantId { get; set; } = null!;
        public DateTime FechaPago { get; set; }
    }
}
