using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
        Task<List<T>> GetAllListAsync();
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllQuery();
    }
}
