using Application.DTOs.Subasta;
using Application.Mappings;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class ObtenerSubastasActivas
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasActivas(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<SubastaDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var subastas = await _subastaRepository.GetActivasAsync( cancellationToken);

            if (subastas == null || !subastas.Any())
            {
                return Enumerable.Empty<SubastaDto>();
            }

            return subastas.Select(s => s.ToDto());

        }
    }
}
