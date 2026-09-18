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

        public async Task<Billetera?> GetByIdWithTransaccionesAsync(int billeteraId)
        {
            return await _context.Billeteras
                .Include(b => b.Transacciones)
                .FirstOrDefaultAsync(b => b.Id == billeteraId);
        }

        public async Task UpdateAsync(Billetera billetera, CancellationToken cancellationToken= default)
        {
            _context.Billeteras.Update(billetera);
            foreach (var transaccion in billetera.Transacciones)
            {
                if (transaccion.Id == 0) 
                {
                    _context.Entry(transaccion).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
