using Application.Dtos.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IIndicatorService
    {
        Task<bool> AddAsync(IndicatorDto dto);
        Task<bool> UpdateAsync(IndicatorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IndicatorDto?> GetById(int id);
        Task<List<IndicatorDto>> GetAll();
        Task<List<IndicatorDto>> GetAllWithIncluded();

        // Extras para calculos
        Task<List<IndicatorDto>> GetByCountryAndYear(int countryId, int year);
        Task<List<int>> GetDistinctYears();
        Task<IndicatorDto?> GetByCountryYearAndMacro(int countryId, int year, int macroId);
        Task<List<IndicatorDto>> GetByMacroAndYear(int macroId, int year);
    }
}
