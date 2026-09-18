using Application.DTOs.Subasta;
using Application.Mappings;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class ObtenerSubastaPorId
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastaPorId(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<SubastaDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            var subasta = await _subastaRepository.GetByIdAsync(id, cancellationToken);

            if (subasta == null)
            {
                return null;
            }

            return subasta.ToDto();
        }

    }
}
