using Application.DTOs.Usuario;
using Application.UseCases.Usuarios;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ObtenerPerfilUsuario obtenerPerfilUsuario;

        public UsuarioController(ObtenerPerfilUsuario obtenerPerfilUsuario)
        {
            this.obtenerPerfilUsuario = obtenerPerfilUsuario;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var perfil = await obtenerPerfilUsuario.ExecuteAsync(id);

            if (perfil == null)
            {
                return NotFound($"No se encontró el usuario con ID {id}."); 
            }

            return Ok(perfil); 
        }
    }
}
