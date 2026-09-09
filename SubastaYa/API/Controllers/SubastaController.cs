using Application.DTOs.Subasta;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubastaController : ControllerBase
    {
        private readonly ISubastaRepository _repo;

        public SubastaController(ISubastaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetAll()
        {
            var subastas = await _repo.GetAllAsync();

            var result = subastas.Select(MapToDto);

            return Ok(result);
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetActivas()
        {
            var subastas = await _repo.GetActivasAsync();

            return Ok(subastas.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubastaDto>> GetById(int id)
        {
            var subasta = await _repo.GetByIdAsync(id);
            if (subasta == null)
                return NotFound();

            return Ok(MapToDto(subasta));
        }



        [HttpPost]
        public async Task<ActionResult<SubastaDto>> CreateSubasta(SubastaDto subastaDto)
        {
            // 1. Map DTO -> Entity
            var subasta = MapToEntity(subastaDto);

            // 2. Persist to database
            await _repo.AddAsync(subasta);

            // 3. Map Entity -> DTO for response
            return CreatedAtAction(nameof(GetById), new { id = subasta.Id }, MapToDto(subasta));
        }

        // Entity -> DTO
        private static SubastaDto MapToDto(Subasta s)
        {
            return new SubastaDto
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
                Estado = s.Estado
            };
        }

        private static Subasta MapToEntity(SubastaDto dto)
        {
            return new Subasta(
                dto.VendedorId,
                dto.CategoriaId,
                dto.Titulo,
                dto.Descripcion,
                dto.UrlImagen,
                dto.PrecioInicial,
                dto.IncrementoMinimo,
                dto.FechaInicio,
                dto.FechaFin
            );
        }
    }
}
