using Application.DTOs.Billetera;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Billeteras
{
    public class ObtenerBilletera
    {
        private readonly IBilleteraRepository _billeteraRepository;

        public ObtenerBilletera(IBilleteraRepository billeteraRepository)
        {
            _billeteraRepository = billeteraRepository;
        }

        public async Task<BilleteraDto?> ExecuteAsync(int usuarioId)
        {
            var billetera = await _billeteraRepository.GetByUsuarioIdAsync(usuarioId);

            if (billetera == null)
            {
                return null;
            }
            return new BilleteraDto
            {
                Id = billetera.Id,
                UsuarioId = billetera.UsuarioId,
                SaldoTotal = billetera.SaldoTotal,
                SaldoRetenido = billetera.SaldoRetenido,
                SaldoDisponible = billetera.SaldoDisponible,
            };
        }
    }
}
