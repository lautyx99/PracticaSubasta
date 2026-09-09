using Application.DTOs.Subasta;
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

        public async Task<SubastaDto?> ExecuteAsync(int id)
        {
            var subasta = await _subastaRepository.GetByIdAsync(id);

            if (subasta == null)
            {
                return null;
            }

            return new SubastaDto
            {
                Id = subasta.Id,
                VendedorId = subasta.VendedorId,
                CategoriaId = subasta.CategoriaId,
                Titulo = subasta.Titulo,
                Descripcion = subasta.Descripcion,
                FechaInicio = subasta.FechaInicio,
                FechaFin = subasta.FechaFin
            };
        }

    }
}
