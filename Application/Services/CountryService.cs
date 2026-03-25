using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Dtos.Country;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IBaseRepository<Country> _repository; 
        public CountryService(IBaseRepository<Country> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(CountryDto dto)
        {
            try
            {
                Country entity = new() { IdCountry = 0, Name = dto.Name, ISOCode = dto.ISOCode };
                Country? returnEntity = await _repository.AddAsync(entity);

                if (returnEntity == null)
                   return false;
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(CountryDto dto)
        {
            try
            {
                Country entity = new() { IdCountry = dto.IdCountry, Name = dto.Name, ISOCode = dto.ISOCode };
                Country? returnEntity = await _repository.UpdateAsync(entity.IdCountry, entity);

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
                var entity = await _repository.GetAllQuery()
                    .Include(c => c.Indicators)
                    .FirstOrDefaultAsync(c => c.IdCountry == id);

                if (entity == null)
                    return false;

                // Se valida si tiene indicadores
                if (entity.Indicators != null && entity.Indicators.Any())
                {
                    // no permitimos borrar
                    return false;
                }

                // Si no tiene indicadores, se puede eliminar
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CountryDto?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                    return null;
                
                CountryDto dto = new() { IdCountry = entity.IdCountry, Name = entity.Name, ISOCode = entity.ISOCode };
                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<CountryDto>> GetAll()
        {
            try
            {
                var listEntities = await _repository.GetAllListAsync();

                var listEntitiesDtos = listEntities.Select(s => new CountryDto() 
                { 
                    IdCountry = s.IdCountry,
                    Name = s.Name,
                    ISOCode = s.ISOCode 
                }).ToList();

                return listEntitiesDtos;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<List<CountryDto>> GetAllWithIncluded()
        {
            try
            {
                var listEntitiesQuery = _repository.GetAllQuery();

                var listEntities = await listEntitiesQuery.Include(c => c.Indicators).ToListAsync();

                var listEntityDtos = listEntities.Select(s =>
                new CountryDto() { 
                    IdCountry = s.IdCountry, 
                    Name = s.Name, 
                    ISOCode = s.ISOCode, 
                    IndicatorsQuantity = s.Indicators != null ? s.Indicators.Count : 0
                }).ToList();

                return listEntityDtos;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

    }
}
