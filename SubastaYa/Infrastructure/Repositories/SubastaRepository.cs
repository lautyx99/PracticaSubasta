using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class SubastaRepository : ISubastaRepository
    {
        private readonly SubastaContext _context;

        public SubastaRepository(SubastaContext context)
        {
            _context = context;
        }

        public async Task<Subasta?> GetByIdAsync(int id)
        {
            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Subasta>> GetAllAsync()
        {
            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .ToListAsync();
        }

        public async Task<List<Subasta>> GetActivasAsync()
        {
            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .Where(s => s.Estado == "Activa")
                .ToListAsync();
        }

        public async Task AddAsync(Subasta subasta)
        {
            await _context.Subastas.AddAsync(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subasta subasta)
        {
            _context.Subastas.Update(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Subasta subasta)
        {
            _context.Subastas.Remove(subasta);
            await _context.SaveChangesAsync();
        }
    }
}
