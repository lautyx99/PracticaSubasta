using Application.DTOs.Subasta;
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

        public async Task<IEnumerable<SubastaDto>> ExecuteAsync()
        {
            var subastas = await _subastaRepository.GetActivasAsync();

            if (subastas == null || !subastas.Any())
            {
                return Enumerable.Empty<SubastaDto>();
            }

            return subastas.Select(s => new SubastaDto
            {
                Id = s.Id,
                VendedorId = s.VendedorId,
                CategoriaId = s.CategoriaId,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin
            });

        }
    }
}
