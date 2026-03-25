using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Country
{
    public class CountryViewModel
    {
        public int IdCountry { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string ISOCode { get; set; } = string.Empty;

        // Para mostrar la cantidad de indicadores 
        public int? IndicatorsQuantity { get; set; }
    }
}
