using Application.DTOs.Puja;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Pujas
{
    public class RealizarPuja
    {
        private readonly ISubastaRepository subastaRepository;
        private readonly IBilleteraRepository billeteraRepository;
        private readonly IPujaRepository pujaRepository;
        private readonly IAuditoriaRepository auditoriaRepository;
        private readonly IUnitOfWork unitOfWork;

        private readonly INotificadorSubasta _notificadorSubasta;

        public RealizarPuja(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            IPujaRepository pujaRepository,
            IAuditoriaRepository auditoriaRepository,
            IUnitOfWork unitOfWork,
            INotificadorSubasta notificadorSubasta)
        {
            this.subastaRepository = subastaRepository;
            this.billeteraRepository = billeteraRepository;
            this.pujaRepository = pujaRepository;
            this.auditoriaRepository = auditoriaRepository;
            this.unitOfWork = unitOfWork;
            this._notificadorSubasta = notificadorSubasta;  
        }

        public async Task<PujaResultadoDto> ExecuteAsync(int subastaId, CrearPujaDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // Iniciar Transacción ACID atómica
            await using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Obtener Subasta
                var subasta = await subastaRepository.GetByIdAsync(subastaId, cancellationToken);
                if (subasta == null)
                {
                    throw new KeyNotFoundException($"[CODE-ERROR] - La subasta con ID {subastaId} no existe.");
                }

                // 2. Validar Estado de Dominio
                if (!subasta.EstaActiva())
                {
                    throw new InvalidOperationException("[CODE-ERROR] - La subasta no se encuentra activa para recibir ofertas.");
                }

                if (subasta.VendedorId == dto.CompradorId)
                {
                    throw new InvalidOperationException("[CODE-ERROR] - El vendedor no puede pujar en su propia subasta.");
                }

                // 3. Validar Monto Mínimo de Oferta
                var pujaMax = await pujaRepository.GetUltimaPujaAsync(subastaId);
                decimal montoMinimoRequerido = (pujaMax != null)
                    ? pujaMax.Monto + subasta.IncrementoMinimo
                    : subasta.PrecioInicial;

                if (dto.Monto < montoMinimoRequerido)
                {
                    throw new InvalidOperationException($"[CODE-ERROR] - El monto debe ser al menos {montoMinimoRequerido}.");
                }

                // 4. Liberar Saldo del Líder Anterior (si aplica)
                if (pujaMax != null)
                {
                    var billeteraAnterior = await billeteraRepository.GetByUsuarioIdAsync(pujaMax.CompradorId);
                    if (billeteraAnterior != null)
                    {
                        billeteraAnterior.LiberarFondos(pujaMax.Monto);
                        await billeteraRepository.UpdateAsync(billeteraAnterior, cancellationToken);
                    }
                }

                // 5. Retener Saldo al Nuevo Líder
                var billeteraNuevoLider = await billeteraRepository.GetByUsuarioIdAsync(dto.CompradorId);
                if (billeteraNuevoLider == null)
                {
                    throw new KeyNotFoundException($"[CODE-ERROR] - No se encontro la billetera del usuario {dto.CompradorId}.");
                }

                billeteraNuevoLider.RetenerFondos(dto.Monto);
                await billeteraRepository.UpdateAsync(billeteraNuevoLider, cancellationToken);

                // 6. Regla Anti-Sniping (Extensión de tiempo en los últimos 60 segundos)
                bool tiempoExtendido = false;
                var segundosRestantes = (subasta.FechaFin - DateTime.UtcNow).TotalSeconds;

                if (segundosRestantes <= 60 && segundosRestantes > 0)
                {
                    var nuevaFechaCierre = subasta.FechaFin.AddMinutes(2);
                    subasta.ExtenderTiempo(nuevaFechaCierre);
                    tiempoExtendido = true;

                    var detalleAntiSniping = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        subastaId = subastaId,
                        fechaAnterior = subasta.FechaFinOriginal,
                        nuevaFechaFin = nuevaFechaCierre
                    });

                    await auditoriaRepository.AddAsync(new AuditoriaLog(
                        usuarioId: dto.CompradorId,
                        entidad: "Subasta",
                        entidadId: subastaId,
                        accion: "ANTI_SNIPING_EXTENDIDO",
                        detalleJson: detalleAntiSniping,
                        fecha: DateTime.UtcNow,
                        servicio: "SubastaService"
                    ));
                }

                // 7. Registrar la nueva Puja
                var nuevaPuja = new Puja(subastaId, dto.CompradorId, dto.Monto, DateTime.UtcNow);
                await pujaRepository.AddAsync(nuevaPuja);

                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();


                var resultadoDto = new PujaResultadoDto
                {
                    Id = nuevaPuja.Id,
                    SubastaId = subastaId,
                    CompradorId = dto.CompradorId,
                    Monto = dto.Monto,
                    FechaUtc = nuevaPuja.Fecha_Puja,
                    TiempoExtendido = tiempoExtendido,
                    NuevaFechaFin = subasta.FechaFin
                };

                await _notificadorSubasta.NotificarNuevaPujaAsync(subastaId, resultadoDto);

                if (tiempoExtendido)
                {
                    await _notificadorSubasta.NotificarTiempoExtendidoAsync(subastaId, subasta.FechaFin);
                }

                return resultadoDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                var detalleError = System.Text.Json.JsonSerializer.Serialize(new
                {
                    mensaje = $"[CODE-ERROR] - Error al procesar puja en subasta {subastaId}: {ex.Message}"
                });

                try
                {
                    await auditoriaRepository.AddAsync(new AuditoriaLog(
                        usuarioId: dto.CompradorId,
                        entidad: "Subasta",
                        entidadId: subastaId,
                        accion: "PUJA_RECHAZADA",
                        detalleJson: detalleError,
                        fecha: DateTime.UtcNow,
                        servicio: "SubastaService"
                    ));

                    await unitOfWork.SaveChangesAsync(); 
                }
                catch (Exception )
                {
                   
                }

                throw; 
            }
        }
    

    }
}
