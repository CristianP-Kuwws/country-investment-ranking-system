using Application.Dtos.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICountryService
    {
        Task<bool> AddAsync(CountryDto dto);
        Task<bool> UpdateAsync(CountryDto dto);
        Task<bool> DeleteAsync(int id);
        Task<CountryDto?> GetById(int id);
        Task<List<CountryDto>> GetAll();
        Task<List<CountryDto>> GetAllWithIncluded(); 
    }
}
