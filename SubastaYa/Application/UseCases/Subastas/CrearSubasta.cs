using Application.DTOs.Subasta;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Subastas
{
    public class CrearSubasta
    {
        private readonly ISubastaRepository subastaRepository;

        private readonly IUnitOfWork unitOfWork;

        public CrearSubasta(ISubastaRepository subastaRepository, IUnitOfWork unitOfWork)
        {
            this.subastaRepository = subastaRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<SubastaDto> ExecuteAsync(CrearSubastaDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // 1. Validaciones básicas de negocio a nivel de entrada
            if (dto.PrecioInicial <= 0)
            {
                throw new ArgumentException("El precio inicial debe ser mayor a cero.", nameof(dto.PrecioInicial));
            }

            if (dto.IncrementoMinimo <= 0)
            {
                throw new ArgumentException("El incremento mínimo debe ser mayor a cero.", nameof(dto.IncrementoMinimo));
            }

            if (dto.FechaFin <= dto.FechaInicio)
            {
                throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.", nameof(dto.FechaFin));
            }

            // 2. Creación de la Entidad de Dominio (encapsula las reglas de inicialización)
            var subasta = new Subasta(
                vendedorId: dto.VendedorId,
                categoriaId: dto.CategoriaId,
                titulo: dto.Titulo,
                descripcion: dto.Descripcion,
                urlImagen: dto.UrlImagen,
                precioInicial: dto.PrecioInicial,
                incrementoMinimo: dto.IncrementoMinimo,
                fechaInicio: dto.FechaInicio,
                fechaFin: dto.FechaFin
            );

            // 3. Persistencia mediante Repositorio y Unit of Work
            await subastaRepository.AddAsync(subasta);
            await unitOfWork.SaveChangesAsync();

            // 4. Mapeo al DTO de salida
            return MapToDto(subasta);
        }

        private static SubastaDto MapToDto(Subasta s)
        {
            return new SubastaDto
            {
                Id = s.Id,
                VendedorId = s.VendedorId,
                CategoriaId = s.CategoriaId,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                UrlImagen = s.UrlImagen,
                PrecioInicial = s.PrecioInicial,
                IncrementoMinimo = s.IncrementoMinimo,
                FechaInicio = s.FechaInicio,
                FechaFin = s.FechaFin,
                Estado = s.Estado.ToString(),
                Version = s.Version
            };
        }

    }
}
