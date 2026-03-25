using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Country
{
    public class CountryDto
    {
        public int IdCountry { get; set; }
        public required string Name { get; set; } = string.Empty;
        public required string ISOCode { get; set; } = string.Empty;

        //
        public int? IndicatorsQuantity { get; set; }    
    }
}
