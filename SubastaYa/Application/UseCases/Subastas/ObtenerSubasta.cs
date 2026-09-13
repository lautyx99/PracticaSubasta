using Application.DTOs.Subasta;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class ObtenerSubasta
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerSubasta(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }
        public async Task<IEnumerable<SubastaDto>> ExecuteAsync( CancellationToken cancellationToken = default)
        {
            var subastas = await _subastaRepository.GetAllAsync(cancellationToken);

            if (subastas == null)
            {
                return Enumerable.Empty<SubastaDto>();
            }
            return subastas.Select(s => new SubastaDto
            {
                Id = s.Id,
                VendedorId = s.VendedorId,
                VendedorNombre = s.Vendedor.Nombre,
                CategoriaId = s.CategoriaId,
                CategoriaNombre = s.Categoria?.Nombre,       
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                PrecioInicial = s.PrecioInicial,
                IncrementoMinimo = s.IncrementoMinimo,      
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin,
                Estado = s.Estado.ToString(),
                GanadorId = s.GanadorId,
                PrecioFinal = s.PrecioFinal,
                                                          
            });
        }
    }
}
