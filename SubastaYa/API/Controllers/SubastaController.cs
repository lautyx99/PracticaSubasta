using Application.DTOs.Subasta;
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

        private static SubastaDto MapToDto(Domain.Entities.Subasta s)
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
    }
}
