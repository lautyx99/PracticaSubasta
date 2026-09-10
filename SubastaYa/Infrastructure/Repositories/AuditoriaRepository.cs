using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly SubastaContext _context;

        public AuditoriaRepository(SubastaContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditoriaLog auditoria)
        {
            await _context.Set<AuditoriaLog>().AddAsync(auditoria);
        }

        public async Task<IEnumerable<AuditoriaLog>> ObtenerPorEntidadAsync(string entidad, int entidadId)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.Entidad == entidad && a.EntidadId == entidadId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }
    }
}
