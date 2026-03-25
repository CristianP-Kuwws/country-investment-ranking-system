using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Indicator
    {
        public int IdIndicator { get; set; }
        [Required]
        public required int IdCountry { get; set; }  // FK
        [Required]
        public required int IdMacroIndicator { get; set; } // FK

        [Required(ErrorMessage = "El valor es requerido.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "El año es requerido.")]
        public int Year { get; set; }

        // Navigation Property
        public Country Country { get; set; } = null;
        public MacroIndicator MacroIndicator { get; set; } = null;

    }
}
