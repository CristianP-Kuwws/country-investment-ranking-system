using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulation
{
    public class EligibleCountryDto
    {
        public int IdCountry { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int IndicatorsCount { get; set; }
    }
}
