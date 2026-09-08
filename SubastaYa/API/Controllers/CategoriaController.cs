using Application.DTOs.Categoria;
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
        private readonly ICategoriaRepository _repo;

        public CategoriaController(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
        {
            var categorias = await _repo.GetAllAsync();
            var categoriasDto = categorias.Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            });
            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDto>> GetById(int id)
        {
            var categoria = await _repo.GetByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };

            return Ok(new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                UrlIcono= categoria.UrlIcono
            });
        }

    }
}
