using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Application.UseCases.Pujas;
using Application.UseCases.Subastas;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
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

        private readonly ObtenerSubastasProximas _obtenerSubastasProximas;

        private readonly EliminarSubasta _eliminarSubasta;

        private readonly ObtenerMisPujas _obtenerMisPujas;
        private readonly ObtenerMisPublicaciones _obtenerMisPublicaciones;

        public SubastaController(
            ObtenerSubasta obtenerSubasta,
            ObtenerSubastaPorId obtenerSubastaPorId,
            CrearSubasta crearSubasta,
            ObtenerSubastasActivas obtenerSubastasActivas,
            EliminarSubasta eliminarSubasta,
            ObtenerMisPublicaciones obtenerMisPublicaciones,
            ObtenerMisPujas obtenerMisPujas,
            ObtenerSubastasProximas obtenerSubastasProximas)
        {
            _obtenerSubasta = obtenerSubasta;
            _obtenerSubastaPorId = obtenerSubastaPorId;
            _crearSubasta = crearSubasta;
            _obtenerSubastasActivas = obtenerSubastasActivas;
            _eliminarSubasta = eliminarSubasta;
            _obtenerMisPublicaciones = obtenerMisPublicaciones;
            _obtenerMisPujas = obtenerMisPujas;
            _obtenerSubastasProximas = obtenerSubastasProximas;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id, [FromQuery] int usuarioId, [FromQuery] bool esAdmin = false, CancellationToken cancellationToken = default)
        {
            try
            {
                await _eliminarSubasta.ExecuteAsync(id, usuarioId, esAdmin, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("mis-pujas")]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetMisPujas(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized();
            }

            var resultado = await _obtenerMisPujas.ExecuteAsync(usuarioId, cancellationToken);
            return Ok(resultado);
        }

        [HttpGet("mis-publicaciones")]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetMisPublicaciones(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized();
            }

            var resultado = await _obtenerMisPublicaciones.ExecuteAsync(usuarioId, cancellationToken);
            return Ok(resultado);
        }

        [HttpGet("proximas")]
        public async Task<ActionResult<IEnumerable<Subasta>>> GetProximas(
        [FromServices] ObtenerSubastasProximas obtenerSubastasProximas,
        CancellationToken cancellationToken)
        {
            var subastas = await obtenerSubastasProximas.ExecuteAsync(cancellationToken);
            return Ok(subastas);
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
