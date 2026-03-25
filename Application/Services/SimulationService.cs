using Application.Dtos;
using Application.Dtos.Country;
using Application.Dtos.Simulation;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Calculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly IMacroIndicatorService _macroIndicatorService;
        private readonly IRankingCalculationService _rankingCalculationService;

        public SimulationService(
            IMacroIndicatorService macroIndicatorService,
            IRankingCalculationService rankingCalculationService)
        {
            _macroIndicatorService = macroIndicatorService;
            _rankingCalculationService = rankingCalculationService;
        }

        // Agregar MacroIndicador a simulacion
        public async Task<bool> AddMacroToSimulation(
            List<MacroWithWeightDto> currentConfig,
            int idMacroIndicator,
            decimal weight)
        {
            try
            {
                
                // Si ya existe en la configuracion, se devuelve falso
                if (currentConfig.Any(c => c.IdMacroIndicator == idMacroIndicator))
                    return false;

                // Suma de pesos actuales
                var currentWeight = currentConfig.Sum(x => x.Weight);

                // La suma no puede superar 1
                if (currentWeight + weight > 1m)
                    return false;

                // Se verifica que exista en bd
                var macroIndicator = await _macroIndicatorService.GetById(idMacroIndicator);
                if (macroIndicator == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Actualizar peso de un MacroIndicador
        public async Task<bool> UpdateMacroInSimulation(
            List<MacroWithWeightDto> currentConfig,
            int idMacroIndicator,
            decimal newWeight)
        {
            try
            {
                // Filtra todos los macros excepto el que se esta actualizando, y posteriormente se suman
                var otherWeight = currentConfig
                    .Where(c => c.IdMacroIndicator != idMacroIndicator)
                    .Sum(c => c.Weight);

                // Se devuelve true si se cumple la condicion
                return (otherWeight + newWeight) <= 1m;
            }
            catch
            {
                return false;
            }
        }

        // Simulacion
        public async Task<(bool Success, string ErrorMessage, List<RankingResultDto> Results)> RunSimulation(
            List<MacroWithWeightDto> configuration,
            int year)
        {
            return await _rankingCalculationService.CalculateRanking(year, configuration);
        }
    }
}
