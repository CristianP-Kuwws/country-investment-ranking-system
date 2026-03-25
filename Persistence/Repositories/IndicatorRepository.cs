using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class IndicatorRepository : IBaseRepository<Indicator>
    {
        private readonly HorizonFutureVestContext _context;

        public IndicatorRepository(HorizonFutureVestContext context)
        {
            _context = context;
        }

        public async Task<Indicator?> AddAsync(Indicator indicator)
        {
            try
            {
                await _context.Set<Indicator>().AddAsync(indicator);
                await _context.SaveChangesAsync();
                return indicator;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el Indicador.", ex);
            }

        }

        public async Task<Indicator?> UpdateAsync(int id, Indicator indicator)
        {
            try
            {
                var entry = await _context.Set<Indicator>().FindAsync(id);

                if (entry != null)
                {
                    _context.Entry(entry).CurrentValues.SetValues(indicator);
                    await _context.SaveChangesAsync();
                    return entry;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Indicador con Id {id}.", ex);

            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entry = await _context.Set<Indicator>().FindAsync(id);

                if (entry != null)
                {
                    _context.Set<Indicator>().Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Indicador con Id {id}.", ex);
            }

        }

        public async Task<List<Indicator>> GetAllListAsync()
        {
            try
            {
                return await _context.Set<Indicator>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Indicadores.", ex);

            }
        }

        public async Task<Indicator?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Indicator>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Indicador con Id {id}.", ex);
            }
        }

        public IQueryable<Indicator> GetAllQuery()
        {
            try
            {
                return _context.Set<Indicator>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la consulta de Indicadores.", ex);
            }
        }

        
    }
}
