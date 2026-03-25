using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.MacroIndicator
{
    public class MacroIndicatorDto
    {
        public int IdMacroIndicator { get; set; }
        public required string Name { get; set; }
        public required decimal Weight { get; set; }
        public required bool IsHighBetter { get; set; } 

        // Cantidad de indicadores
        public int? IndicatorsQuantity { get; set; }

    }
}
