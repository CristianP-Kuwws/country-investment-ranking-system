using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.MacroIndicator
{
    public class SaveMacroIndicatorViewModel
    {
        public int IdMacroIndicator { get; set; }

        [Required(ErrorMessage = "El Nombre del MacroIndicador es requerido.")]
        public required string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Peso del MacroIndicador es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor a 0.")]
        public required decimal Weight { get; set; }

        [Required(ErrorMessage = "Se requiere saber si el MacroIndicador es mejor alto o no.")]
        public required bool IsHighBetter { get; set; }

        // Para mostrar la cantidad de indicadores 
        public int? IndicatorsQuantity { get; set; }

        // Para mostrar el peso restante
        public decimal RemainingWeight { get; set; }

    }
}
