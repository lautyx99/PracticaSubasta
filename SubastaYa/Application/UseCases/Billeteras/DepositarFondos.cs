using Application.DTOs.Billetera;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Billeteras
{
    public class DepositarFondos
    {
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepositarFondos(
            IBilleteraRepository billeteraRepository,
            IUnitOfWork unitOfWork)
        {
            _billeteraRepository = billeteraRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BilleteraDto?> ExecuteAsync(DepositoDto dto, CancellationToken cancellationToken = default)
        {
            // 1. Validar reglas de entrada básicas
            if (dto.Monto <= 0)
            {
                throw new ArgumentException("El monto a depositar debe ser mayor a cero.", nameof(dto.Monto));
            }

            // 2. Obtener la billetera asociada al usuario
            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(dto.UsuarioId);
            if (billetera == null)
            {
                return null;
            }

            // 3. Aplicar la regla de negocio del Dominio (Aumenta el saldo y genera la TransaccionLedger)
            billetera.Depositar(dto.Monto);

            // 4. Notificar los cambios al repositorio y persistir la transacción
            await _billeteraRepository.UpdateAsync(billetera, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            // 5. Mapear y retornar el DTO de respuesta actualizado
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
