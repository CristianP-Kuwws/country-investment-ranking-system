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
    public class CountryRepository : IBaseRepository<Country>
    {
        private readonly HorizonFutureVestContext _context;

        public CountryRepository(HorizonFutureVestContext context)
        {
            _context = context;
        }

        public async Task<Country?> AddAsync(Country country)
        {
            try
            {
                await _context.Set<Country>().AddAsync(country);
                await _context.SaveChangesAsync();
                return country;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al agregar el Pais.", ex);
            }
            
        }

        public async Task<Country?> UpdateAsync(int id, Country country)
        {

            try
            {
                var entry = await _context.Set<Country>().FindAsync(id);

                if (entry != null)
                {
                    _context.Entry(entry).CurrentValues.SetValues(country); // aislar transaccion (var obtenido)
                    await _context.SaveChangesAsync();
                    return entry;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Pais con Id {id}.", ex);
            }
            
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entry = await _context.Set<Country>().FindAsync(id);

                if (entry != null)
                {
                    _context.Set<Country>().Remove(entry);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Pais con Id {id}.", ex);
            }
         
        }

        public async Task<List<Country>> GetAllListAsync()
        {
            try
            {
                return await _context.Set<Country>().ToListAsync(); //EF - Deferred Execution

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de Paises.", ex);
            }
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Country>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Pais con Id {id}.", ex);
            }
        }

        public IQueryable<Country> GetAllQuery()
        {
            try
            {
                return _context.Set<Country>().AsQueryable();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la consulta de Paises.", ex);
            }

        }
    }
}
