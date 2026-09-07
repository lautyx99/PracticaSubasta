using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IPujaRepository
    {
        Task<Puja?> GetByIdAsync(int id);
        Task<List<Puja>> GetBySubastaIdAsync(int subastaId);
        Task AddAsync(Puja puja);
    }
}
