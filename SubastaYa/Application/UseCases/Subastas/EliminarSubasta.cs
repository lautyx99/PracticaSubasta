using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class EliminarSubasta
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditoriaService _auditoriaService;

        public EliminarSubasta(
            ISubastaRepository subastaRepository,
            IPujaRepository pujaRepository,
            IUnitOfWork unitOfWork,
            IAuditoriaService auditoriaService)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
            _unitOfWork = unitOfWork;
            _auditoriaService = auditoriaService;
        }

        public async Task ExecuteAsync(int subastaId, int usuarioId, bool esAdmin, CancellationToken cancellationToken = default)
        {
            // 1. Buscar la subasta
            var subasta = await _subastaRepository.GetByIdAsync(subastaId, cancellationToken);
            if (subasta == null)
            {
                throw new KeyNotFoundException($"La subasta con ID {subastaId} no existe.");
            }

            // 2. Validar permisos (solo el creador o un administrador pueden eliminarla)
            if (!esAdmin && subasta.VendedorId != usuarioId)
            {
                throw new UnauthorizedAccessException("No tienes permisos para eliminar esta subasta.");
            }

            // 3. Validar regla de negocio: No se puede eliminar si ya recibió ofertas
            var ultimaPuja = await _pujaRepository.GetUltimaPujaAsync(subastaId);
            if (ultimaPuja != null)
            {
                throw new InvalidOperationException("No se puede eliminar una subasta que ya cuenta con ofertas registradas.");
            }

            // 4. Cambiar estado a Cancelada / Eliminar
            subasta.MarcarComoCancelada(); // O el método de borrado físico que maneje tu repositorio
            await _subastaRepository.UpdateAsync(subasta, cancellationToken);

            // 5. 📝 Registrar la eliminación en el sistema de auditoría
            await _auditoriaService.RegistrarEventoAsync(
                usuarioId: usuarioId,
                entidad: "Subasta",
                entidadId: subasta.Id,
                accion: "ELIMINACION_SUBASTA",
                detalles: new
                {
                    Motivo = "Eliminación de subasta sin ofertas previas",
                    EjecutadoPorAdmin = esAdmin
                },
                servicio: "SubastaService"
            );

            // 6. Persistir cambios
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
