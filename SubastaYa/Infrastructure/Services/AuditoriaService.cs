using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly SubastaContext _context;

        public AuditoriaService(IAuditoriaRepository auditoriaRepository, SubastaContext context)
        {
            _auditoriaRepository = auditoriaRepository;
            _context = context;
        }

        public async Task RegistrarEventoAsync(int? usuarioId, string entidad, int entidadId, string accion, object detalles, string servicio)
        {
            string detalleJson = System.Text.Json.JsonSerializer.Serialize(detalles);

            var log = new AuditoriaLog(usuarioId, entidad, entidadId, accion, detalleJson, DateTime.UtcNow, servicio);

            await _auditoriaRepository.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
