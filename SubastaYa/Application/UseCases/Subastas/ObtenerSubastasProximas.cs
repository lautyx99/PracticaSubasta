using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class ObtenerSubastasProximas
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubastasProximas(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<Subasta>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var ahora = DateTime.UtcNow;
            return await _subastaRepository.GetProximasAsync(ahora, cancellationToken);
        }
    }
}
