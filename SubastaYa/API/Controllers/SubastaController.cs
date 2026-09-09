using Application.DTOs.Subasta;
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

        public SubastaController(ObtenerSubasta obtenerSubasta, ObtenerSubastaPorId obtenerSubastaPorId, CrearSubasta crearSubasta, ObtenerSubastasActivas obtenerSubastasActivas)
        {
            this.obtenerSubasta = obtenerSubasta;
            this.obtenerSubastaPorId = obtenerSubastaPorId;
            this.crearSubasta = crearSubasta;
            this.obtenerSubastasActivas = obtenerSubastasActivas;

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

      
    }
}
