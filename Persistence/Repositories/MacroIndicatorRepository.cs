using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class MacroIndicatorRepository : IBaseRepository<MacroIndicator>
    {
        private readonly HorizonFutureVestContext _context;

        public MacroIndicatorRepository(HorizonFutureVestContext context)
        {
            _context = context;
        }

        public async Task<MacroIndicator?> AddAsync(MacroIndicator macroIndicator)
        {
            try
            {
                await _context.Set<MacroIndicator>().AddAsync(macroIndicator);
                await _context.SaveChangesAsync();
                return macroIndicator;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el MacroIndicador.", ex);
            }
        }

        public async Task<MacroIndicator?> UpdateAsync(int id, MacroIndicator macroIndicator)
        {
            try
            {
                var entry = await _context.Set<MacroIndicator>().FindAsync(id);

                if (entry != null)
                {
                    _context.Entry(entry).CurrentValues.SetValues(macroIndicator);
                    await _context.SaveChangesAsync();
                    return entry;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el MacroIndicador con Id {id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entry = await _context.Set<MacroIndicator>().FindAsync(id);

                if (entry != null)
                {
                    _context.Set<MacroIndicator>().Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el MacroIndicador con Id {id}.", ex);
            }
        }

        public async Task<List<MacroIndicator>> GetAllListAsync()
        {
            try
            {
                return await _context.Set<MacroIndicator>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de MacroIndicadores.", ex);
            }
        }

        public async Task<MacroIndicator?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<MacroIndicator>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el MacroIndicador con Id {id}.", ex);
            }
        }

        public IQueryable<MacroIndicator> GetAllQuery()
        {
            try
            {
                return _context.Set<MacroIndicator>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la consulta de MacroIndicadores.", ex);
            }
        }
    }
}
