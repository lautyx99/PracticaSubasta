using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface ISubastaRepository
    {
        Task<Subasta?> GetByIdAsync(int id);
        Task<List<Subasta>> GetAllAsync();
        Task<List<Subasta>> GetActivasAsync();
        Task AddAsync(Subasta subasta);
        Task UpdateAsync(Subasta subasta);
        Task DeleteAsync(Subasta subasta);
    }
}
