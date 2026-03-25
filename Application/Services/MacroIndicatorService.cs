using Application.Dtos.MacroIndicator;
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
    public class MacroIndicatorService : IMacroIndicatorService
    {
        private readonly IBaseRepository<MacroIndicator> _repository;
        public MacroIndicatorService(IBaseRepository<MacroIndicator> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(MacroIndicatorDto dto)
        {
            try
            {
                MacroIndicator entity = new()
                {
                    IdMacroIndicator = 0,
                    Name = dto.Name,
                    Weight = dto.Weight,
                    IsHighBetter = dto.IsHighBetter
                };

                MacroIndicator? returnEntity = await _repository.AddAsync(entity);

                if (returnEntity == null)
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(MacroIndicatorDto dto)
        {
            try
            {
                MacroIndicator entity = new()
                {
                    IdMacroIndicator = dto.IdMacroIndicator,
                    Name = dto.Name,
                    Weight = dto.Weight,
                    IsHighBetter = dto.IsHighBetter
                };
                
                MacroIndicator? returnEntity = await _repository.UpdateAsync(entity.IdMacroIndicator, entity);

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
                    .Include(mi => mi.Indicators)
                    .FirstOrDefaultAsync(mi => mi.IdMacroIndicator == id);

                if (entity == null)
                    return false;

                // Se valida si tiene indicadores
                if (entity.Indicators != null && entity.Indicators.Any())
                {
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

        public async Task<MacroIndicatorDto?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                    return null;

                MacroIndicatorDto dto = new()
                {
                    IdMacroIndicator = entity.IdMacroIndicator,
                    Name = entity.Name,
                    Weight = entity.Weight,
                    IsHighBetter = entity.IsHighBetter
                };

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<MacroIndicatorDto>> GetAll()
        {
            try
            {
               var listEntities = await _repository.GetAllListAsync();

               var listEntitiesDtos = listEntities.Select(m =>
               new MacroIndicatorDto
               {
                   IdMacroIndicator = m.IdMacroIndicator, Name = m.Name,
                   Weight = m.Weight, IsHighBetter = m.IsHighBetter
               }).ToList();    

                return listEntitiesDtos;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<List<MacroIndicatorDto>> GetAllWithIncluded()
        {
            try
            {
                var listEntitiesQuery = _repository.GetAllQuery();
                var listEntities = await listEntitiesQuery.Include(mi => mi.Indicators).ToListAsync();

                var listEntitiesDto = listEntities.Select(s =>
                    new MacroIndicatorDto()
                    {
                        IdMacroIndicator = s.IdMacroIndicator,
                        Name = s.Name,
                        Weight = s.Weight,
                        IsHighBetter = s.IsHighBetter,
                        IndicatorsQuantity = s.Indicators?.Count ?? 0
                    }).ToList();

                return listEntitiesDto;
            }
            catch (Exception ex)
            {
                return [];
            }
        }
    }
}
