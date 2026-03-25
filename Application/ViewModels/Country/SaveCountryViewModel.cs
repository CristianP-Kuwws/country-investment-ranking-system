using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Country
{
    public class SaveCountryViewModel
    {
        public int IdCountry { get; set; }

        [Required(ErrorMessage = "El Nombre del pais es requerido.")]
        public required string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El codigo ISO del pais es requerido.")]
        [StringLength(3, ErrorMessage = "El código ISO no puede superar los 3 caracteres.")]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "El codigo ISO debe contener exactamente 3 letras mayusculas.")]
        public required string ISOCode { get; set; } = string.Empty;

        // Para mostrar la cantidad de indicadores 
        public int? IndicatorsQuantity { get; set; }
    }
}
