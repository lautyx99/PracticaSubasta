using Application.DTOs.Transaccion;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Billeteras
{
    public class ObtenerMovimientos
    {
        private readonly IBilleteraRepository _billeteraRepository;

        public ObtenerMovimientos(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<IEnumerable<TransaccionDto>> ExecuteAsync(int billeteraId)
        {
            // Consultas la billetera incluyendo sus transacciones
            var billetera = await _billeteraRepository.GetByIdWithTransaccionesAsync(billeteraId);

            if (billetera == null || billetera.Transacciones == null || !billetera.Transacciones.Any())
            {
                return Enumerable.Empty<TransaccionDto>();
            }

            return billetera.Transacciones.Select(t => new TransaccionDto
            {
                Id = t.Id,
                BilleteraId = t.BilleteraId,
                Tipo = t.Tipo,
                Monto = t.Monto,
                Fecha = t.Fecha,
                SubastaId = t.SubastaId
            });
        }
    }
}
