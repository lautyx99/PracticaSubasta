using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public static class SubastaMappingExtensions
    {
        public static SubastaDto ToDto(this Subasta subasta)
        {
            if (subasta == null) return null!;

            var ultimaPuja = subasta.Pujas?
                .OrderByDescending(p => p.Monto)
                .FirstOrDefault();

            return new SubastaDto
            {
                Id = subasta.Id,
                VendedorId = subasta.VendedorId,
                VendedorNombre = subasta.Vendedor?.Nombre, // Corregido (sin duplicado)
                CategoriaId = subasta.CategoriaId,
                CategoriaNombre = subasta.Categoria?.Nombre,
                Titulo = subasta.Titulo,
                Descripcion = subasta.Descripcion,
                UrlImagen = subasta.UrlImagen,
                PrecioInicial = subasta.PrecioInicial,
                IncrementoMinimo = subasta.IncrementoMinimo,
                FechaInicio = subasta.FechaInicio,
                FechaFin = subasta.FechaFin,
                Estado = subasta.Finalizada ? EstadoSubasta.Finalizada.ToString() : subasta.Estado.ToString(),
                GanadorId = subasta.GanadorId,
                PrecioFinal = ultimaPuja != null ? ultimaPuja.Monto : subasta.PrecioFinal,
                Version = subasta.Version,
                MejorPuja = ultimaPuja != null ? new PujaDto
                {
                    Id = ultimaPuja.Id,
                    SubastaId = ultimaPuja.SubastaId,
                    CompradorId = ultimaPuja.CompradorId,
                    CompradorNombre = ultimaPuja.Usuario?.Nombre, // Corregido (sin duplicado)
                    Monto = ultimaPuja.Monto,
                    FechaPuja = ultimaPuja.Fecha_Puja
                } : null
            };
        }
     }
}

