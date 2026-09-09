using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Pujas
{
    public class ObtenerPuja
    {
        private readonly IPujaRepository _pujaRepository;

        public ObtenerPuja(IPujaRepository pujaRepository)
        {
            _pujaRepository = pujaRepository;
        }

        // Se cambia el parámetro a int subastaId
        public async Task<IEnumerable<PujaDto>> ExecuteAsync(int subastaId)
        {
            var pujas = await _pujaRepository.GetBySubastaIdAsync(subastaId);

            // Si no hay pujas, el repositorio o LINQ devolverá una lista vacía, no null.
            if (pujas == null || !pujas.Any())
            {
                return Enumerable.Empty<PujaDto>();
            }

            return pujas.Select(p => new PujaDto
            {
                Id = p.Id,
                SubastaId = p.SubastaId,
                Monto = p.Monto,
                FechaPuja = p.Fecha_Puja,
            });
        }
    }
}
