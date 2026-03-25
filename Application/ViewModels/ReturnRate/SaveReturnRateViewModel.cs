using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReturnRate
{
    public class SaveReturnRateViewModel
    {
        public int IdReturnRate { get; set; } // cuenta con las validaciones abajo, funcionan raro.

        [Required(ErrorMessage = "La tasa minima es requerida")]
        [Range(0.01, 100, ErrorMessage = "La tasa minima debe estar entre 0.01 y 100")]
        public decimal MinReturnRate { get; set; }

        [Required(ErrorMessage = "La tasa maxima es requerida")]
        [Range(0.01, 100, ErrorMessage = "La tasa maxima debe estar entre 0.01 y 100")]
        public decimal MaxReturnRate { get; set; }
    }
}
