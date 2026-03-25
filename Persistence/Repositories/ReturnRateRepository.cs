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
    public class ReturnRateRepository : IBaseRepository<ReturnRate>
    {
        private readonly HorizonFutureVestContext _context;

        public ReturnRateRepository(HorizonFutureVestContext context)
        {
            _context = context;
        }

        public async Task<ReturnRate?> AddAsync(ReturnRate returnRate)
        {
            try
            {
                await _context.Set<ReturnRate>().AddAsync(returnRate);
                await _context.SaveChangesAsync();
                return returnRate;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la Tasa de Retorno.", ex);
            }
        }

        public async Task<ReturnRate?> UpdateAsync(int id, ReturnRate returnRate)
        {
            try
            {
                var entry = await _context.Set<ReturnRate>().FindAsync(id);

                if (entry != null)
                {
                    _context.Entry(entry).CurrentValues.SetValues(returnRate);
                    await _context.SaveChangesAsync();
                    return entry;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la Tasa de Retorno con Id {id}.", ex);
            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entry = await _context.Set<ReturnRate>().FindAsync(id);

                if (entry != null)
                {
                    _context.Set<ReturnRate>().Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la Tasa de Retorno con Id {id}.", ex);
            }

        }

        public async Task<List<ReturnRate>> GetAllListAsync()
        {
            try
            {
                return await _context.Set<ReturnRate>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Tasas de Retorno.", ex);
            }
        }
        public async Task<ReturnRate?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ReturnRate>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la Tasa de Retorno con Id {id}.", ex);
            }
        }

        public IQueryable<ReturnRate> GetAllQuery()
        {
            try
            {
                return _context.Set<ReturnRate>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la consulta de Tasas de Retorno.", ex);
            }
        }
    }
}
