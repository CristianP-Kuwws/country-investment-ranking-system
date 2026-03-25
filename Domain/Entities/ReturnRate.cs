using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReturnRate
    {
        public int IdReturnRate { get; set; }

        [Required(ErrorMessage = "Tasa Minima de Retorno es requerida.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Debe ser mayor a 0.")]
        public required decimal MinReturnRate { get; set; }

        [Required(ErrorMessage = "Tasa Maxima de Retorno es requerida.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Debe ser mayor a 0.")]
        public required decimal MaxReturnRate { get; set; }

    }
}
