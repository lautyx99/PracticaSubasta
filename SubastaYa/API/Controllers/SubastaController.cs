using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Application.UseCases.Pujas;
using Application.UseCases.Subastas;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubastaController : ControllerBase
    {
        private readonly ObtenerSubasta _obtenerSubasta;
        private readonly ObtenerSubastaPorId _obtenerSubastaPorId;
        private readonly CrearSubasta _crearSubasta;
        private readonly ObtenerSubastasActivas _obtenerSubastasActivas;

        public SubastaController(
            ObtenerSubasta obtenerSubasta,
            ObtenerSubastaPorId obtenerSubastaPorId,
            CrearSubasta crearSubasta,
            ObtenerSubastasActivas obtenerSubastasActivas)
        {
            _obtenerSubasta = obtenerSubasta;
            _obtenerSubastaPorId = obtenerSubastaPorId;
            _crearSubasta = crearSubasta;
            _obtenerSubastasActivas = obtenerSubastasActivas;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetAll()
        {
            var subastas = await _obtenerSubasta.ExecuteAsync();
            return Ok(subastas);
        }

        [AllowAnonymous]
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetActivas()
        {
            var subastas = await _obtenerSubastasActivas.ExecuteAsync();
            return Ok(subastas);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubastaDto>> GetById(int id)
        {
            var subasta = await _obtenerSubastaPorId.ExecuteAsync(id);

            if (subasta == null)
                return NotFound($"No se encontró la subasta con ID {id}.");

            return Ok(subasta);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<SubastaDto>> CreateSubasta([FromBody] CrearSubastaRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int vendedorId))
            {
                return Unauthorized();
            }

            var dto = new CrearSubastaDto
            {
                Titulo = request.Titulo,
                Descripcion = request.Descripcion,
                UrlImagen = request.UrlImagen,
                PrecioInicial = request.PrecioInicial,
                IncrementoMinimo = request.IncrementoMinimo,
                CategoriaId = request.CategoriaId,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                VendedorId = vendedorId
            };

            var nuevaSubasta = await _crearSubasta.ExecuteAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = nuevaSubasta.Id }, nuevaSubasta);
        }

        public record CrearSubastaRequest(
            string Titulo,
            string Descripcion,
            string? UrlImagen,
            decimal PrecioInicial,
            decimal IncrementoMinimo,
            int CategoriaId,
            DateTime FechaInicio,
            DateTime FechaFin);
    }
}
