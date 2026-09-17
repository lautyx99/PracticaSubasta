using Application.DTOs.Puja;
using Application.UseCases.Pujas;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subastas/{subastaId}/pujas")] // <-- Ruta base estandarizada
    public class PujaController : ControllerBase
    {
        private readonly ObtenerPuja obtenerPuja;
        private readonly RealizarPuja realizarPujaUseCase;

        public PujaController(ObtenerPuja obtenerPuja, RealizarPuja realizarPujaUseCase)
        {
            this.obtenerPuja = obtenerPuja;
            this.realizarPujaUseCase = realizarPujaUseCase;
        }

        /// <summary>
        /// GET: api/subastas/{subastaId}/pujas
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PujaDto>>> GetBySubasta([FromRoute] int subastaId)
        {
            var pujas = await obtenerPuja.ExecuteAsync(subastaId);
            return Ok(pujas);
        }

        /// <summary>
        /// GET: api/subastas/{subastaId}/pujas/{pujaId}
        /// </summary>
        [HttpGet("{pujaId:int}")]
        [ProducesResponseType(typeof(PujaResultadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPujaPorId([FromRoute] int subastaId, [FromRoute] int pujaId)
        {
            return Ok();
        }

        /// <summary>
        /// POST: api/subastas/{subastaId}/pujas
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PujaResultadoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegistrarPuja(
            [FromRoute] int subastaId,
            [FromBody] CrearPujaRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int compradorId))
            {
                return Unauthorized();
            }

            var dto = new CrearPujaDto
            {
                CompradorId = compradorId,
                Monto = request.Monto
            };

            try
            {

                PujaResultadoDto resultado = await realizarPujaUseCase.ExecuteAsync(subastaId, dto);

                // Corregido: controllerName apunta a "Puja" para coincidir con PujaController
                return CreatedAtAction(
                    actionName: nameof(ObtenerPujaPorId),
                    controllerName: "Puja",
                    routeValues: new { subastaId = subastaId, pujaId = resultado.Id },
                    value: resultado);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                // 🎯 Forzamos el HTTP 409 Conflict ante colisiones de concurrencia optimista
                return Conflict(new { mensaje = "Conflicto de concurrencia: la subasta fue modificada por otra puja simultánea." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        public record CrearPujaRequest(decimal Monto);
    }
}
