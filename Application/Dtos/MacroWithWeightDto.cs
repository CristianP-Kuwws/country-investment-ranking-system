using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class MacroWithWeightDto
    {
        public int IdMacroIndicator { get; set; }
        public required string Name { get; set; }
        public required decimal Weight { get; set; }
        public required bool IsHighBetter { get; set; }
    }
}
