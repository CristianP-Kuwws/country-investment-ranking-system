using Application.Dtos.Indicator;
using Application.Dtos.ReturnRate;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ReturnRateService : IReturnRateService
    {

        private readonly IBaseRepository<ReturnRate> _repository;
        public ReturnRateService(IBaseRepository<ReturnRate> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddAsync(ReturnRateDto dto)
        {
            try
            {
                ReturnRate entity = new()
                {
                    IdReturnRate = 0,
                    MinReturnRate = dto.MinReturnRate,
                    MaxReturnRate = dto.MaxReturnRate
                };

                ReturnRate? returnEntity = await _repository.AddAsync(entity);

                if (returnEntity == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ReturnRateDto dto)
        {
            try
            {
                ReturnRate entity = new()
                {
                    IdReturnRate = dto.IdReturnRate,
                    MinReturnRate = dto.MinReturnRate,
                    MaxReturnRate = dto.MaxReturnRate
                };

                ReturnRate? returnEntity = await _repository.UpdateAsync(entity.IdReturnRate, entity);

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

        public async Task<ReturnRateDto?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);

                if (entity == null)
                    return null;

                ReturnRateDto dto = new()
                {
                    IdReturnRate = entity.IdReturnRate,
                    MinReturnRate = entity.MinReturnRate,
                    MaxReturnRate = entity.MaxReturnRate
                };

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ReturnRateDto>> GetAll()
        {
            try
            {
                var listEntities = await _repository.GetAllListAsync();

                var listEntitiesDtos = listEntities.Select(s => new ReturnRateDto
                {
                    IdReturnRate = s.IdReturnRate,
                    MinReturnRate = s.MinReturnRate,
                    MaxReturnRate = s.MaxReturnRate
                }).ToList();

                return listEntitiesDtos;
            }
            catch (Exception ex)
            {
                return [];
            }
        }

    }
}
