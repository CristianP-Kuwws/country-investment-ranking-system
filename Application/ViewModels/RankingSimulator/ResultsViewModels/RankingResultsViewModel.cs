using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulator.ResultsViewModels
{
    public class RankingResultsViewModel
    {
        public int Year { get; set; }
        public List<RankingItemViewModel> Rankings { get; set; } = new();
        public string? SingleCountryName { get; set; }
    }
}
