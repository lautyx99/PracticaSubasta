using Application.DTOs.Subasta;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class ObtenerMisPublicaciones
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerMisPublicaciones(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<SubastaDto>> ExecuteAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            // 1. Llamamos al repositorio
            var subastas = await _subastaRepository.GetByVendedorIdAsync(usuarioId, cancellationToken);

            return subastas.Select(s => new SubastaDto
            {
                Id = s.Id,
                VendedorId = s.VendedorId,
                VendedorNombre = s.Vendedor.Nombre,
                CategoriaId = s.CategoriaId,
                CategoriaNombre = s.Categoria?.Nombre,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                UrlImagen = s.UrlImagen,
                PrecioInicial = s.PrecioInicial,
                IncrementoMinimo = s.IncrementoMinimo,
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin,
                Estado = s.Estado.ToString(),
                GanadorId = s.GanadorId,
                PrecioFinal = s.PrecioFinal,
                CantidadPujas = s.Pujas.Count
            });
        }
    }
}
