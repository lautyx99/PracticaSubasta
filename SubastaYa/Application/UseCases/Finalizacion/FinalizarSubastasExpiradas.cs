using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Finalizacion
{
    public class FinalizarSubastasExpiradas
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IPujaRepository _pujaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditoriaService _auditoriaService;
        private readonly INotificadorSubasta _notificadorSubasta;

        public FinalizarSubastasExpiradas(
            ISubastaRepository subastaRepository,
            IPujaRepository pujaRepository,
            IBilleteraRepository billeteraRepository,
            IUnitOfWork unitOfWork,
            INotificadorSubasta notificadorSubasta,
            IAuditoriaService auditoriaService)
        {
            _subastaRepository = subastaRepository;
            _pujaRepository = pujaRepository;
            _billeteraRepository = billeteraRepository;
            _unitOfWork = unitOfWork;
            _notificadorSubasta = notificadorSubasta;
            _auditoriaService = auditoriaService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            // 1. Obtener subastas activas que ya expiraron
            var subastasExpiradas = await _subastaRepository.GetSubastasExpiradasSinFinalizarAsync(cancellationToken);

            foreach (var subasta in subastasExpiradas)
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

                try
                {
                    // 2. Obtener la última puja (oferta ganadora)
                    var ultimaPuja = await _pujaRepository.GetUltimaPujaAsync(subasta.Id);

                    if (ultimaPuja != null)
                    {
                        // 3. Liquidar la venta
                        var billeteraVendedor = await _billeteraRepository.GetByUsuarioIdAsync(subasta.VendedorId);
                        var billeteraComprador = await _billeteraRepository.GetByUsuarioIdAsync(ultimaPuja.CompradorId);

                        if (billeteraVendedor != null && billeteraComprador != null)
                        {
                            // Transferir el saldo retendido al vendedor
                            billeteraComprador.ConfirmarDebito(ultimaPuja.Monto);
                            billeteraVendedor.AcreditarVenta(ultimaPuja.Monto);

                            await _billeteraRepository.UpdateAsync(billeteraComprador, cancellationToken);
                            await _billeteraRepository.UpdateAsync(billeteraVendedor, cancellationToken);
                        }

                        subasta.MarcarComoFinalizada(ganadorId: ultimaPuja.CompradorId, precioFinal: ultimaPuja.Monto);
                    }
                    else
                    {
                        // Subasta finalizada sin ofertas
                        subasta.MarcarComoFinalizada(ganadorId: null, precioFinal: null);
                    }

                    await _subastaRepository.UpdateAsync(subasta, cancellationToken);

                    await _auditoriaService.RegistrarEventoAsync(
                    usuarioId: 0, // 0 indica que fue ejecutado por el Sistema / Worker
                    entidad: "Subasta",
                    entidadId: subasta.Id,
                    accion: "CAMBIO_ESTADO",
                    detalles: new
                    {
                        EstadoAnterior = "Activa",
                        EstadoNuevo = "Finalizada",
                        GanadorId = subasta.GanadorId,
                        PrecioFinal = subasta.PrecioFinal,
                        Motivo = "Cierre automático por tiempo expirado (Worker)"
                    },
                        servicio: "FinalizarSubastasExpiradas"
                     );

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    // 4. Notificar a los clientes conectados vía SignalR
                    await _notificadorSubasta.NotificarSubastaFinalizadaAsync(subasta.Id, subasta.GanadorId, subasta.PrecioFinal);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    // Opcional: Registrar en logs el fallo específico de esta subasta
                }
            }
        }
    }
}
