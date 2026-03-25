using Application.ViewModels.Country;
using Application.ViewModels.MacroIndicator;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Indicator
{
    public class SaveIndicatorViewModel
    {
        public int IdIndicator { get; set; }

        [Required(ErrorMessage = "El pais es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un país valido.")]
        public required int IdCountry { get; set; }       // FK

        [Required(ErrorMessage = "El MacroIndicador es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un macroindicador valido.")]
        public required int IdMacroIndicator { get; set; } // FK

        [Required(ErrorMessage = "El valor es requerido.")]
        [Range(0.0001, 99999.9999, ErrorMessage = "El valor debe ser mayor a 0.")]

        public required decimal Value { get; set; }

        [Required(ErrorMessage = "El año es requerido.")]
        [Range(1900, 2100, ErrorMessage = "Debe ingresar un año valido.")]
        public required int Year { get; set; }

        // mostrar el nombre del pais o macroindicador
        public string? CountryName { get; set; }
        public string? MacroIndicatorName { get; set; }

        // Listas de soporte
        public List<CountryViewModel>? Countries { get; set; } = new();
        public List<MacroIndicatorViewModel>? MacroIndicators { get; set; } = new();

    }
}
