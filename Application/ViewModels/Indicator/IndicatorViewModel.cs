using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Indicator
{
    public class IndicatorViewModel
    {
        public int IdIndicator { get; set; }
        public required int IdCountry { get; set; }       // FK
        public required int IdMacroIndicator { get; set; } // FK
        public required decimal Value { get; set; }
        public required int Year { get; set; }

        // mostrar el nombre del pais y macroindicador
        public string? CountryName { get; set; }
        public string? MacroIndicatorName { get; set; }

    }
}
