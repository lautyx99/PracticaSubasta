using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly SubastaContext _context;

        public CategoriaRepository(SubastaContext context)
        {
            _context = context;
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}
