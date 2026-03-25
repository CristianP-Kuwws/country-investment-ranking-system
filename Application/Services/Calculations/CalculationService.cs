using Application.Dtos;
using Application.Dtos.Country;
using Application.Dtos.Indicator;
using Application.Dtos.Simulation;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Calculations;

namespace Application.Services.Calculations
{
    public class RankingCalculationService : IRankingCalculationService
    {

        private readonly IIndicatorService _indicatorService;
        private readonly ICountryService _countryService;
        private readonly IReturnRateService _returnRateService;

        private const decimal weightTolerance = 0.0001m; // margen para validar la suma de pesos
        private const decimal defaultMinRate = 2m;
        private const decimal defaultMaxRate = 15m;

        public RankingCalculationService(
            IIndicatorService indicatorService,
            ICountryService countryService,
            IReturnRateService returnRateService)
        {
            _indicatorService = indicatorService;
            _countryService = countryService;
            _returnRateService = returnRateService;
        }
        

        // Calculo general de ranking
        public async Task<(bool Success, string ErrorMessage, List<RankingResultDto> Results)>
            CalculateRanking(int year, List<MacroWithWeightDto> macros)
        {
            // Suma de pesos (configuracion recibida)
            var totalWeight = macros.Sum(c => c.Weight);

            if (Math.Abs(totalWeight - 1m) > weightTolerance)
            {
                return (false,
                    "Se deben ajustar los pesos de los macroindicadores registrado hasta que la suma de los mismo sea igual a 1",
                    new List<RankingResultDto>());
            }

            // Obtener paises elegibles y validar
            var eligibleCountries = await GetEligibleCountries(year, macros);

            if (eligibleCountries.Count == 0)
            {
                var requiredMacrosNames = macros
                    .Where(m => m.Weight > 0)
                    .Select(m => m.Name)
                    .ToList();

                var macrosText = string.Join(", ", requiredMacrosNames);

                return (false,
                    $"No hay paises elegibles para el año {year}. " +
                    $"Para que un pais sea elegible, debe tener valores registrados para todos los siguientes macroindicadores: {macrosText}. " +
                    $"Verifique que los paises tengan estos indicadores en el año seleccionado.",
                    new List<RankingResultDto>());
            }

            if (eligibleCountries.Count == 1)
            {
                var countryName = eligibleCountries.First().Name;
                return (false,
                    $"No hay suficientes paises para poder calcular el ranking y la tasa de retorno, " +
                    $"el unico pais que cumple con los requisitos es {countryName}, " +
                    $"debe agregar mas indicadores a los demas paises en el año seleccionado",
                    new List<RankingResultDto>());
            }

            // Calcular ranking final 
            var rankings = await CalculateRankings(year, macros, eligibleCountries);

            return (true, "", rankings);
        }

        // Paises que cumplen con los macros seleccionados
        private async Task<List<CountryDto>> GetEligibleCountries(
            int year,
            List<MacroWithWeightDto> macros)
        {
            try
            {
                // Lista de macroIds en la configuracion cuyo peso sea mayor a 0
                var requiredMacroIds = macros
                    .Where(c => c.Weight > 0)
                    .Select(c => c.IdMacroIndicator)
                    .ToList();

                var countries = await _countryService.GetAll();
                var eligibleCountries = new List<CountryDto>();

                foreach (var country in countries)
                {
                    var indicators = await _indicatorService.GetByCountryAndYear(country.IdCountry, year);

                    // Indicadores en ese año (distinct para eliminar duplicados)
                    var macroIds = indicators.Select(i => i.IdMacroIndicator).Distinct().ToList();

                    // Comprobar que los macros requeridos estén presentes
                    if (requiredMacroIds.All(id => macroIds.Contains(id)))
                    {
                        eligibleCountries.Add(country);
                    }
                }

                return eligibleCountries;
            }
            catch
            {
                return new List<CountryDto>();
            }
        }

        // Ranking final
        private async Task<List<RankingResultDto>> CalculateRankings(
            int year,
            List<MacroWithWeightDto> macros,
            List<CountryDto> countries)
        {
            var results = new List<RankingResultDto>();

            foreach (var country in countries)
            {
                var score = await CalculateCountryScoring(country.IdCountry, year, macros, countries);

                var returnRate = await CalculateReturnRate(score);

                results.Add(new RankingResultDto
                {
                    IdCountry = country.IdCountry,
                    CountryName = country.Name,
                    IsoCode = country.ISOCode,
                    Scoring = score,
                    EstimatedReturnRate = returnRate
                });
            }

            return results.OrderByDescending(r => r.Scoring).ToList();
        }

        // Puntaje de un pais
        private async Task<decimal> CalculateCountryScoring(
            int countryId,
            int year,
            List<MacroWithWeightDto> macros,
            List<CountryDto> eligibleCountries)
        {
            decimal totalScore = 0;

            foreach (var macro in macros.Where(m => m.Weight > 0))
            {
                var indicator = await _indicatorService.GetByCountryYearAndMacro(countryId, year, macro.IdMacroIndicator);
                if (indicator == null) continue;

                var normalized = await NormalizeIndicatorValue(
                    indicator.Value,
                    macro.IdMacroIndicator,
                    year,
                    macro.IsHighBetter,
                    eligibleCountries
                );

                totalScore += normalized * macro.Weight;
            }

            if (totalScore < 0 || totalScore > 1)
            {
                throw new InvalidOperationException(
                    $"Error en cálculo: scoring fuera de rango (0-1). Valor: {totalScore}");
            }

            return totalScore;
        }

        // Normalizar valor de indicador (entre 0 y 1 con paises elegibles)
        private async Task<decimal> NormalizeIndicatorValue(
            decimal value,
            int macroIndicatorId,
            int year,
            bool isHighBetter,
            List<CountryDto> eligibleCountries)
        {
            // Filtrar indicadores de países elegibles
            var indicators = new List<IndicatorDto>();
            foreach (var country in eligibleCountries)
            {
                var ind = await _indicatorService.GetByCountryYearAndMacro(country.IdCountry, year, macroIndicatorId);
                if (ind != null)
                {
                    indicators.Add(ind);
                }
            }

            if (!indicators.Any())
                return 0;

            var min = indicators.Min(i => i.Value);
            var max = indicators.Max(i => i.Value);

            if (min == max)
                return 0.5m;

            return isHighBetter
                ? (value - min) / (max - min)
                : (max - value) / (max - min);
        }

        // Calcular tasa de retorno 
        private async Task<decimal> CalculateReturnRate(decimal scoring)
        {
            decimal minRate = defaultMinRate;
            decimal maxRate = defaultMaxRate;

            // Obtener tasas configuradas
            var rates = await _returnRateService.GetAll();
            var configuredRate = rates.FirstOrDefault();

            if (configuredRate != null &&
                configuredRate.MinReturnRate > 0 &&
                configuredRate.MaxReturnRate > 0)
            {
                minRate = configuredRate.MinReturnRate;
                maxRate = configuredRate.MaxReturnRate;
            }

            // r = rmin + (rmax - rmin) × Sp
            return minRate + ((maxRate - minRate) * scoring);
        }
    }
}
