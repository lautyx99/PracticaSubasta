using Application.DTOs.Puja;
using Application.DTOs.Subasta;
using Application.UseCases.Pujas;
using Application.UseCases.Subastas;
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
        private readonly ObtenerSubasta obtenerSubasta;

        private readonly ObtenerSubastaPorId obtenerSubastaPorId;

        private readonly CrearSubasta crearSubasta;

        private readonly ObtenerSubastasActivas obtenerSubastasActivas;

        private readonly RealizarPuja realizarPuja;

        public SubastaController(ObtenerSubasta obtenerSubasta, ObtenerSubastaPorId obtenerSubastaPorId, CrearSubasta crearSubasta, ObtenerSubastasActivas obtenerSubastasActivas, RealizarPuja realizarPuja)
        {
            this.obtenerSubasta = obtenerSubasta;
            this.obtenerSubastaPorId = obtenerSubastaPorId;
            this.crearSubasta = crearSubasta;
            this.obtenerSubastasActivas = obtenerSubastasActivas;
            this.realizarPuja = realizarPuja;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetAll()
        {
            var subastas = await obtenerSubasta.ExecuteAsync();

            return Ok(subastas);
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<SubastaDto>>> GetActivas()
        {
            var subastas = await obtenerSubastasActivas.ExecuteAsync();

            return Ok(subastas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubastaDto>> GetById(int id)
        {
            var subasta = await obtenerSubastaPorId.ExecuteAsync(id);

            if (subasta == null)
                return NotFound($"No se encontró la subasta con ID {id}.");

            return Ok(subasta);
        }



        // GET /api/v1/subastas/{subastaId}/pujas
        [HttpGet("{subastaId:int}/pujas")]
        public async Task<ActionResult> GetPujasPorSubasta([FromRoute] int subastaId)
        {
            // Delegación al Caso de Uso ObtenerHistorialPujas...
            return Ok();
        }



        [HttpPost]
        public async Task<ActionResult<SubastaDto>> CreateSubasta([FromBody] CrearSubastaDto subastaDto)
        {
            if (subastaDto == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            var nuevaSubasta = await crearSubasta.ExecuteAsync(subastaDto);

            // Devuelve 201 Created con la subasta generada
            return CreatedAtAction(nameof(GetById), new { id = nuevaSubasta.Id }, nuevaSubasta);
        }


        // POST /api/v1/subastas/{subastaId}/pujas
        [HttpPost("{subastaId:int}/pujas")]
        public async Task<ActionResult<PujaResultadoDto>> RegistrarPuja(
            [FromRoute] int subastaId,
            [FromBody] CrearPujaDto dto)
        {
            var resultado = await realizarPuja.ExecuteAsync(subastaId, dto);
            return CreatedAtAction(nameof(GetPujasPorSubasta), new { subastaId = subastaId }, resultado);
        }


    }
}
