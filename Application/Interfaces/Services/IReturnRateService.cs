using Application.Dtos.ReturnRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IReturnRateService
    {
        Task<bool> AddAsync(ReturnRateDto dto);
        Task<bool> UpdateAsync(ReturnRateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ReturnRateDto?> GetById(int id);
        Task<List<ReturnRateDto>> GetAll();
    }
}
