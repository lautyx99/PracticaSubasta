using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class PujaRepository : IPujaRepository
    {
        private readonly SubastaContext _context;

        public PujaRepository(SubastaContext context)
        {
            _context = context;
        }

        public async Task<Puja?> GetByIdAsync(int id)
        {
            return await _context.Pujas.FindAsync(id);
        }

        public async Task<List<Puja>> GetBySubastaIdAsync(int subastaId)
        {
            return await _context.Pujas
                .Where(p => p.SubastaId == subastaId)
                .OrderByDescending(p => p.Monto)
                .ToListAsync();
        }

        public async Task AddAsync(Puja puja)
        {
            await _context.Pujas.AddAsync(puja);
        }

        public async Task<Puja?> GetUltimaPujaAsync(int subastaId)
        {
            return await _context.Pujas
                 .Where(p => p.SubastaId == subastaId)
                 .OrderByDescending(p => p.Fecha_Puja) // O por p.Id descendente
                 .FirstOrDefaultAsync();
        }
    }
}
