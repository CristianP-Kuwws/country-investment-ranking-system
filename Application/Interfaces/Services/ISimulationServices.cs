using Application.Dtos;
using Application.Dtos.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ISimulationService
    {
        Task<bool> AddMacroToSimulation(List<MacroWithWeightDto> currentConfig, int idMacroIndicator, decimal weight);
        Task<bool> UpdateMacroInSimulation(List<MacroWithWeightDto> currentConfig, int idMacroIndicator, decimal newWeight);

        Task<(bool Success, string ErrorMessage, List<RankingResultDto> Results)> RunSimulation(
            List<MacroWithWeightDto> configuration,
            int year);
    }
}
