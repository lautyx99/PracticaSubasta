using Application.DTOs.Categoria;
using Application.UseCases.Categorias;
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
    public class CategoriaController : ControllerBase
    {
        private readonly  ObtenerCategoria obtenerCategoria;

        private readonly ObtenerCategoriaPorId obtenerCategoriaPorId;

        public CategoriaController(ObtenerCategoria obtenerCategoria, ObtenerCategoriaPorId obtenerCategoriaPorId)
        {
            this.obtenerCategoria = obtenerCategoria;
            this.obtenerCategoriaPorId = obtenerCategoriaPorId;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
        {
            var categorias = await obtenerCategoria.ExecuteAsync();

            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDto>> GetById(int id)
        {
            var categoria = await obtenerCategoriaPorId.ExecuteAsync(id);

            if (categoria == null)
            {
                return NotFound($"No se encontró la categoria para la categoria con ID {id}.");
            }

            return Ok(categoria);
        }

    }
}
