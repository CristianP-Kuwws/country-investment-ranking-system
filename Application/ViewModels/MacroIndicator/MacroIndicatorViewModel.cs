using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.MacroIndicator
{
    public class MacroIndicatorViewModel
    {
        public int IdMacroIndicator { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required decimal Weight { get; set; }
        public required bool IsHighBetter { get; set; }

        // Mostrar la cantidad de indicadores asociados
        public int? IndicatorsQuantity { get; set; }
    }
}
