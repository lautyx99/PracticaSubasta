using Application.DTOs.Billetera;
using Application.DTOs.Transaccion;
using Application.UseCases.Billeteras;
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
    public class BilleteraController : ControllerBase
    {
    private readonly ObtenerBilletera _obtenerPerfilBilletera;
    private readonly ObtenerMovimientos _obtenerMovimientosBilletera;
    private readonly DepositarFondos _realizarDeposito;

    public BilleteraController(
        ObtenerBilletera obtenerBilletera,
        ObtenerMovimientos obtenerMovimientos,
        DepositarFondos realizarDeposito)
    {
        _obtenerPerfilBilletera = obtenerBilletera;
        _obtenerMovimientosBilletera = obtenerMovimientos;
        _realizarDeposito = realizarDeposito;
    }

    // GET: api/billeteras?usuarioId=1
    [HttpGet]
    public async Task<ActionResult<BilleteraDto>> GetByUsuario([FromQuery] int usuarioId)
    {
        var billeteraDto = await _obtenerPerfilBilletera.ExecuteAsync(usuarioId);

        if (billeteraDto == null)
        {
            return NotFound($"No se encontró la billetera para el usuario con ID {usuarioId}.");
        }

        return Ok(billeteraDto);
    }

    // GET: api/billeteras/5/transacciones
    [HttpGet("{billeteraId}/transacciones")]
    public async Task<ActionResult<IEnumerable<TransaccionDto>>> GetTransacciones(int billeteraId)
    {
        var transacciones = await _obtenerMovimientosBilletera.ExecuteAsync(billeteraId);

        // Siempre retorna 200 OK (con elementos o un array vacío [])
        return Ok(transacciones);
    }

        [HttpPost("deposito")]
        public async Task<ActionResult<BilleteraDto>> DepositoFondos([FromBody] DepositoDto dto)
        {
            if (dto.Monto <= 0)
            {
                return BadRequest("El monto a depositar debe ser mayor a cero.");
            }

            // Se pasa el objeto dto completo al caso de uso
            var billeteraActualizada = await _realizarDeposito.ExecuteAsync(dto);

            if (billeteraActualizada == null)
            {
                return NotFound($"No se encontró la billetera asociada al usuario con ID {dto.UsuarioId}.");
            }

            return Ok(billeteraActualizada);
        }
    }
    }

