using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Wallaboo.Entities;

namespace Wallaboo.Models
{
    public class RegistroViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }=null!;
        [Required]
        [MaxLength(50)]
        [Display(Name = "Tu nombre, nombre del negocio o servicio que das")]
        public string NombreComercial { get; set; } = null!;

        [Required]
        [Display(Name = "Direccion de tu negocio o servicio")]
        [MaxLength(150)]
        public string DireccionComercial { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numero de telefono para que te contacten")]
        public string TelefonoComercial { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        [Display(Name = "Descripcion de tu negocio o servicio (tienda, kiosko, abogado, particular, etc)")]
        public string DescripcionComercial { get; set; } = null!;

        [Required]
        [Display(Name = "Selecciona tu pais de residencia")]
        public int PaisId { get; set; }

        [Required]
        [Display(Name = "Selecciona tu provincia de residencia")]
        public int ProvinciaId {  get; set; }

        [Required]
        [Display(Name = "Selecciona tu ciudad de residencia")]
        public int CiudadId { get; set; } 
        
        public IEnumerable<Pais> ListaPaises { get; set; }=new List<Pais>();
        //public IEnumerable<Provincia> ListaProvincias { get; set; } = new List<Provincia>();
        //public IEnumerable<Ciudad> ListaCiudades { get; set; } = new List<Ciudad>();

    }
}
