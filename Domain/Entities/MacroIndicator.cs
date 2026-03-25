using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MacroIndicator
    {
        public int IdMacroIndicator { get; set; }

        [Required(ErrorMessage = "El Nombre del Macroindicador es requerido.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El Peso del Macroindicador es requerido.")]
        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1")]
        public required decimal Weight { get; set; }

        [Required(ErrorMessage = "Debe especificar si un valor alto es mejor.")]
        public required bool IsHighBetter { get; set; } 
        public ICollection<Indicator> Indicators { get; set; } = new List<Indicator>();

    }
}
