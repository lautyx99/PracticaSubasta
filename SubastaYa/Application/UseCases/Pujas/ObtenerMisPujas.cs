using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Pujas
{
    public class ObtenerMisPujas
    {
        private readonly ISubastaRepository _subastaRepository;

        public ObtenerMisPujas(ISubastaRepository subastaRepository)
        {
            _subastaRepository = subastaRepository;
        }

        public async Task<IEnumerable<SubastaDto>> ExecuteAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var subastas = await _subastaRepository.GetByUsuarioParticipanteAsync(usuarioId, cancellationToken);

            return subastas.Select(s => new SubastaDto
            {
                Id = s.Id,
                VendedorId = s.VendedorId,
                VendedorNombre = s.Vendedor?.Nombre,
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
                CantidadPujas = s.Pujas != null ? s.Pujas.Count : 0,
                Pujas = s.Pujas?.Select(p => new PujaDto
                {
                    Id = p.Id,
                    SubastaId = p.SubastaId,
                    CompradorId = p.CompradorId,
                    Monto = p.Monto,
                    FechaPuja = p.Fecha_Puja
                }).ToList() ?? new List<PujaDto>()
            });
        }
    }
}
