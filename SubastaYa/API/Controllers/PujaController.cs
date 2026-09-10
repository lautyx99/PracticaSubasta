using Application.DTOs.Puja;
using Application.UseCases.Pujas;
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
    public class PujaController : ControllerBase
    {
        private readonly ObtenerPuja obtenerPuja;

        private readonly RealizarPuja realizarPujaUseCase;

        public PujaController(ObtenerPuja obtenerPuja, RealizarPuja realizarPujaUseCase)
        {
            this.obtenerPuja = obtenerPuja;
            this.realizarPujaUseCase = realizarPujaUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PujaDto>>> GetBySubasta(int subastaId)
        {
            var pujas = await obtenerPuja.ExecuteAsync(subastaId);

            return Ok(pujas);
        }


        /// <summary>
        /// Endpoint de consulta individual de la puja generada.
        /// </summary>
        [HttpGet("{pujaId:int}")]
        [ProducesResponseType(typeof(PujaResultadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPujaPorId(int subastaId, int pujaId)
        {
            // Opcional: Implementar lectura de la puja guardada mediante un Query o Repositorio de lectura
            return Ok();
        }


        /// <summary>
        /// Realiza una nueva oferta en una subasta activa.
        /// </summary>
        /// <param name="subastaId">ID de la subasta objetivo.</param>
        /// <param name="dto">Datos de la oferta (CompradorId, Monto).</param>
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


            // 2. Mapear al DTO del Caso de Uso asociando el ID seguro
            var dto = new CrearPujaDto
            {
                CompradorId = compradorId,
                Monto = request.Monto
            };

            // Ejecutar el caso de uso orquestado (ACID, Locks, SignalR)
            PujaResultadoDto resultado = await realizarPujaUseCase.ExecuteAsync(subastaId, dto);

            // Retornar 201 Created especificando la ruta para consultar la puja realizada
            return CreatedAtAction(
                actionName: nameof(ObtenerPujaPorId),
                controllerName: "Pujas",
                routeValues: new { subastaId = subastaId, pujaId = resultado.Id },
                value: resultado);
        }

        /// <summary>
        /// DTO exclusivo para la Petición HTTP (solo recibe el Monto).
        /// </summary>
        public record CrearPujaRequest(decimal Monto);
    }
}
