using Application.Dtos;
using Application.Dtos.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services.Calculations
{
    public interface IRankingCalculationService
    {
        Task<(bool Success, string ErrorMessage, List<RankingResultDto> Results)>
            CalculateRanking(int year, List<MacroWithWeightDto> macros);
    }

}
