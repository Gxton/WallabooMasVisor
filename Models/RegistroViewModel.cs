using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
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
        [DisplayName("Ingresa el horario de atención")]
        public string HorarioComercial { get; set; } = null!;   

        [Required]
        [Display(Name = "Pais")]
        public int PaisId { get; set; }

        [Required]
        [Display(Name = "Provincia/Estado")]
        public int ProvinciaId {  get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public int CiudadId { get; set; } 
        
        public IEnumerable<Pais> ListaPaises { get; set; }=new List<Pais>();
        public IEnumerable<Provincia> ListaProvincias { get; set; } = new List<Provincia>();
        //public IEnumerable<Ciudad> ListaCiudades { get; set; } = new List<Ciudad>();

    }
}
