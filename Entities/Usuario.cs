using Wallaboo.Interfaces;


namespace Wallabo.Entities
{
    public class Usuario:IEntidadTenant
    {

        //public int Id { get; set; }
        public string NombreComercial { get; set; } = null!;
        public string DireccionComercial { get; set;} = null!;
        public string TelefonoComercial { get; set;} = null!;
        public string? URLComercial { set; get; }
        public byte[]? QRURL { get; set; }
        public string DescripcionComercial { set; get; } = null!;
        public string HorarioComercial { set; get; } = null!;
        public string TenantId { get; set; } = null!;
        public int PaisId {  get; set; }
        public int ProvinciaId { get; set; } 
        public int CiudadId { get; set; }
        //public List<Anuncio> Anuncios { get; set; } = null!;
        
    }
}
