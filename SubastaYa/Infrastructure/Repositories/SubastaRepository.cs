using Domain;
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

        public async Task<Subasta?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subastas
            .Include(s => s.Vendedor)
            .Include(s => s.Categoria)
            .Include(s => s.Pujas)
            .ThenInclude(p => p.Usuario)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Subasta>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subastas
            .Include(s => s.Vendedor)
            .Include(s => s.Categoria)
            .Include(s => s.Pujas)
            .ThenInclude(p => p.Usuario)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        }

        public async Task<List<Subasta>> GetActivasAsync(CancellationToken cancellationToken = default)
        {
            var ahora = DateTime.UtcNow;

            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .ThenInclude(p => p.Usuario)
                .Where(s => s.Estado == EstadoSubasta.Activa
                 && !s.Finalizada
                 && s.FechaFin > ahora)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Subasta subasta)
        {
            await _context.Subastas.AddAsync(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subasta subasta, CancellationToken cancellationToken = default)
        {
            _context.Subastas.Update(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Subasta subasta)
        {
            _context.Subastas.Remove(subasta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Subasta>> GetSubastasExpiradasSinFinalizarAsync(CancellationToken cancellationToken = default)
        {
            var fechaActual = DateTime.UtcNow;

            return await _context.Subastas
                .Where(s => s.FechaFin <= fechaActual && !s.Finalizada) 
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Subasta>> GetByVendedorIdAsync(int vendedorId, CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .Include(s => s.Pujas) 
                .Where(s => s.VendedorId == vendedorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Subasta>> GetByUsuarioParticipanteAsync(int usuarioId, CancellationToken cancellationToken)
        {
            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .Include(s => s.Pujas) 
                .Where(s => s.Pujas.Any(p => p.CompradorId == usuarioId))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Subasta>> GetProximasAsync(DateTime fechaActual, CancellationToken cancellationToken)
        {
            var ahora = DateTime.UtcNow;

            return await _context.Subastas
                .Include(s => s.Vendedor)
                .Include(s => s.Categoria)
                .Include(s => s.Pujas)
                .Where(s => s.FechaInicio > ahora) // O el filtro de estado que utilices para próximas
                .OrderBy(s => s.FechaInicio)
                .ToListAsync(cancellationToken);
        }
    }
}
