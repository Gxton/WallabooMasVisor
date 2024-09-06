using System.Drawing;
using Wallaboo.Interfaces;

namespace Wallaboo.Entities
{
    public class Imagen:IEntidadTenant
    {
        public int Id { get; set; }
        public int AnuncioId { get; set; }
        public string TenantId { get; set; } = null!;
        public string? Image1Path { get; set; }
        public string? Image2Path { get; set; }
        public string? Image3Path { get; set; }
        public string? Image4Path { get; set; }
        public string? Image5Path { get; set; }
    }
}
