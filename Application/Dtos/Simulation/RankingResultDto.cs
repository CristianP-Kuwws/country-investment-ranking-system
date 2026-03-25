using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Simulation
{
    public class RankingResultDto
    {
        public int IdCountry { get; set; }
        public string CountryName { get; set; }
        public string IsoCode { get; set; }
        public decimal Scoring { get; set; }
        public decimal EstimatedReturnRate { get; set; }
    }
}
