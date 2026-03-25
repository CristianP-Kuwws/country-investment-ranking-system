using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Indicator
{
    public class IndicatorDto
    {
        public int IdIndicator { get; set; }
        public required int IdCountry { get; set; }  // FK
        public required int IdMacroIndicator { get; set; } // FK
        public decimal Value { get; set; }
        public int Year { get; set; }

        // Consultar el nombre del pais o macroindicador
        public string? CountryName { get; set; }
        public string? MacroIndicatorName { get; set; }
    }
}
