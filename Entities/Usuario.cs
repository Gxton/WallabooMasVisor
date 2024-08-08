using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallaboo.Entities;
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
        public string DescripcionComercial { set; get; } = null!;
        public string TenantId { get; set; } = null!;
        public string Pais {  get; set; }=null!;
        public string Provincia { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        //public List<Anuncio> Anuncios { get; set; } = null!;
        
    }
}
