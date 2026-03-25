using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulator
{
    public class SimulationMacroViewModel
    {
        public int IdMacroIndicator { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public bool IsHighBetter { get; set; }
    }
}
