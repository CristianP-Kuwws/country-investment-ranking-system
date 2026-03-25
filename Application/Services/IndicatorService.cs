using Application.Dtos.Indicator;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class IndicatorService : IIndicatorService
    {
        private readonly IBaseRepository<Indicator> _repository;
        public IndicatorService(IBaseRepository<Indicator> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(IndicatorDto dto)
        {
            try
            {
                Indicator entity = new()
                {
                    IdIndicator = 0,
                    IdCountry = dto.IdCountry,
                    IdMacroIndicator = dto.IdMacroIndicator,
                    Value = dto.Value,
                    Year = dto.Year
                };

                Indicator? returnEntity = await _repository.AddAsync(entity);

                if (returnEntity == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(IndicatorDto dto)
        {
            try
            {
                Indicator entity = new()
                {
                    IdIndicator = dto.IdIndicator,
                    IdCountry = dto.IdCountry,
                    IdMacroIndicator = dto.IdMacroIndicator,
                    Value = dto.Value,
                    Year = dto.Year
                };

                Indicator? returnEntity = await _repository.UpdateAsync(entity.IdIndicator, entity);

                if (returnEntity == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IndicatorDto?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                    return null;

                IndicatorDto dto = new()
                {
                    IdIndicator = entity.IdIndicator,
                    IdCountry = entity.IdCountry,
                    IdMacroIndicator = entity.IdMacroIndicator,
                    Value = entity.Value,
                    Year = entity.Year
                };

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<IndicatorDto>> GetAll()
        {
            try
            {
                var listEntities = await _repository.GetAllListAsync();

                var listEntitiesDto = listEntities.Select(i => 
                new IndicatorDto() 
                { 
                    IdIndicator = i.IdIndicator,
                    IdCountry = i.IdCountry,
                    IdMacroIndicator = i.IdMacroIndicator,
                    Value = i.Value,
                    Year = i.Year 
                }).ToList();

                return listEntitiesDto;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<List<IndicatorDto>> GetAllWithIncluded()
        {
            try
            {
                var listEntitiesQuery = _repository.GetAllQuery();

                var listEntities = await listEntitiesQuery
                    .Include(i => i.Country)
                    .Include(i => i.MacroIndicator)
                    .ToListAsync();

                var listEntitiesDto = listEntities.Select(i => new IndicatorDto
                {
                    IdIndicator = i.IdIndicator,
                    IdCountry = i.IdCountry,
                    IdMacroIndicator = i.IdMacroIndicator,
                    Value = i.Value,
                    Year = i.Year,
                    CountryName = i.Country != null ? i.Country.Name : "",
                    MacroIndicatorName = i.MacroIndicator != null ? i.MacroIndicator.Name : ""
                }).ToList();

                return listEntitiesDto;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<List<IndicatorDto>> GetByCountryAndYear(int countryId, int year)
        {
            try
            {
                var query = _repository.GetAllQuery();

                var listEntities = await query
                    .Where(i => i.IdCountry == countryId && i.Year == year)
                    .Include(i => i.Country)
                    .Include(i => i.MacroIndicator)
                    .ToListAsync();

                var listDtos = listEntities.Select(i => new IndicatorDto
                {
                    IdIndicator = i.IdIndicator,
                    IdCountry = i.IdCountry,
                    IdMacroIndicator = i.IdMacroIndicator,
                    Value = i.Value,
                    Year = i.Year,
                    CountryName = i.Country != null ? i.Country.Name : string.Empty,
                    MacroIndicatorName = i.MacroIndicator != null ? i.MacroIndicator.Name : string.Empty
                }).ToList();

                return listDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<int>> GetDistinctYears()
        {
            try
            {
                var years = await _repository.GetAllQuery()
                    .Select(i => i.Year)
                    .Distinct()
                    .OrderBy(y => y)
                    .ToListAsync();

                return years;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<IndicatorDto?> GetByCountryYearAndMacro(int countryId, int year, int macroId)
        {
            try
            {
                var entity = await _repository.GetAllQuery()
                    .FirstOrDefaultAsync(i => i.IdCountry == countryId
                                           && i.Year == year
                                           && i.IdMacroIndicator == macroId);

                if (entity == null) return null;

                return new IndicatorDto
                {
                    IdIndicator = entity.IdIndicator,
                    IdCountry = entity.IdCountry,
                    IdMacroIndicator = entity.IdMacroIndicator,
                    Value = entity.Value,
                    Year = entity.Year
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<IndicatorDto>> GetByMacroAndYear(int macroId, int year)
        {
            try
            {
                var entities = await _repository.GetAllQuery()
                    .Where(i => i.IdMacroIndicator == macroId && i.Year == year)
                    .ToListAsync();

                return entities.Select(i => new IndicatorDto
                {
                    IdIndicator = i.IdIndicator,
                    IdCountry = i.IdCountry,
                    IdMacroIndicator = i.IdMacroIndicator,
                    Value = i.Value,
                    Year = i.Year
                }).ToList();
            }
            catch (Exception ex)
            {
                return [];
            }
        }

    }
}
