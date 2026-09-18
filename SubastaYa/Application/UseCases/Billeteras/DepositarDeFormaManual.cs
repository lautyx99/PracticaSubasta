using Application.DTOs.Billetera;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Billeteras
{
    internal class DepositarDeFormaManual
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositarDeFormaManual(
            IBilleteraRepository billeteraRepository,
            IAuditoriaRepository auditoriaRepository,
            IUnitOfWork unitOfWork)
        {
            _billeteraRepository = billeteraRepository;
            _auditoriaRepository = auditoriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BilleteraDto?> ExecuteAsync(int adminId, DepositoManualDto dto, CancellationToken cancellationToken = default)
        {
            // 1. Validar reglas de entrada
            if (dto.Monto <= 0)
            {
                throw new ArgumentException("El monto a acreditar debe ser mayor a cero.", nameof(dto.Monto));
            }

            // 2. Obtener la billetera del usuario afectado
            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(dto.UsuarioAfectadoId);
            if (billetera == null)
            {
                throw new KeyNotFoundException($"No se encontró la billetera para el usuario ID {dto.UsuarioAfectadoId}");
            }

            // 3. Aplicar la acreditación (puedes usar tu método de dominio existente o uno específico de carga manual)
            billetera.Depositar(dto.Monto);
            await _billeteraRepository.UpdateAsync(billetera, cancellationToken);

            // 4. 📝 Registrar obligatoriamente el evento crítico en la Auditoría (Requisito TP)
            var detalleJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                adminId = adminId,
                usuarioAfectadoId = dto.UsuarioAfectadoId,
                montoAcreditado = dto.Monto,
                motivo = dto.Motivo ?? "Acreditación manual de saldo por administración"
            });

            var auditoriaLog = new AuditoriaLog(
                usuarioId: adminId,            
                entidad: "Billetera",
                entidadId: billetera.Id,
                accion: "ACREDITACION_MANUAL", 
                detalleJson: detalleJson,
                fecha: DateTime.UtcNow,
                servicio: "BilleteraAdminService"
            );

            await _auditoriaRepository.AddAsync(auditoriaLog);

            // 5. Persistir todo de forma atómica
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(billetera);
        }

        private static BilleteraDto MapToDto(Billetera billetera)
        {
            return new BilleteraDto
            {
                Id = billetera.Id,
                UsuarioId = billetera.UsuarioId,
                SaldoTotal = billetera.SaldoTotal,
                SaldoRetenido = billetera.SaldoRetenido,
                SaldoDisponible = billetera.SaldoDisponible,
                Version = billetera.Version
            };
        }
    }
}

