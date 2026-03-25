using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Country
    {
        public int IdCountry { get; set; }

        [Required(ErrorMessage = "El Nombre del pais es requerido.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El codigo ISO es requerido.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El codigo ISO debe tener exactamente 3 caracteres.")]
        public required string ISOCode { get; set; }
        public ICollection<Indicator> Indicators { get; set; } = new List<Indicator>();
    }
}
