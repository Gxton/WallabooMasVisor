using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wallaboo.Interfaces;

namespace Wallaboo.Entities
{
    public class Anuncio : IEntidadTenant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Descripcion { get; set; } = null!;

        public string TenantId { get; set; } = null!;

        [Required(ErrorMessage = "La fecha desde es requerida.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Desde")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La fecha hasta es requerida.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Hasta")]
        [DateAfter("FechaDesde", ErrorMessage = "La fecha hasta debe ser posterior a la fecha desde.")]
        public DateTime FechaHasta { get; set; }

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        public int CantidadDias { get; set; }
        public int Activo { get; set; }
        public int Pagado { get; set; }

        // Relación con las imágenes
        public virtual ICollection<Imagen>? Imagenes { get; set; }
    }

    // Clase personalizada para validar que una fecha sea posterior a otra
    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateAfterAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext.ObjectType.GetProperty(_comparisonProperty)
                .GetValue(validationContext.ObjectInstance);

            if (value is DateTime dateValue && comparisonValue is DateTime dateComparison)
            {
                if (dateValue <= dateComparison)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}

