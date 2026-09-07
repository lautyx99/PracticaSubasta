using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class BilleteraRepository : IBilleteraRepository
    {
        private readonly SubastaContext _context;

        public BilleteraRepository(SubastaContext context)
        {
            _context = context;
        }

        public async Task<Billetera?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Billeteras
                .FirstOrDefaultAsync(b => b.UsuarioId == usuarioId);
        }

        public async Task UpdateAsync(Billetera billetera)
        {
            _context.Billeteras.Update(billetera);
            await _context.SaveChangesAsync();
        }
    }
}
