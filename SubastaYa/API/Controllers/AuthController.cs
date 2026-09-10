using Application.DTOs.Auth;
using Application.DTOs.Login;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var resultado = await authService.LoginAsync(request);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }

        [HttpPost("registro")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registro([FromBody] RegistroRequestDto request)
        {
            try
            {
                var resultado = await authService.RegistrarAsync(request);
                return StatusCode(StatusCodes.Status201Created, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
