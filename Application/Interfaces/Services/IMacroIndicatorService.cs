using Application.Dtos.MacroIndicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IMacroIndicatorService
    {
        Task<bool> AddAsync(MacroIndicatorDto dto);
        Task<bool> UpdateAsync(MacroIndicatorDto dto);
        Task<bool> DeleteAsync(int id);
        Task<MacroIndicatorDto?> GetById(int id);
        Task<List<MacroIndicatorDto>> GetAll();
        Task<List<MacroIndicatorDto>> GetAllWithIncluded(); 
    }
}
