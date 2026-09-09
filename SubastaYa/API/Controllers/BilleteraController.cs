using Application.DTOs.Billetera;
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
        private readonly IBilleteraRepository _repo;

        public BilleteraController(IBilleteraRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<BilleteraDto>> GetByUsuario(int usuarioId)
        {
            var billetera = await _repo.GetByUsuarioIdAsync(usuarioId);
            if (billetera == null)
            {
                return NotFound();
            }

            var result = new BilleteraDto
            {
                Id = billetera.Id,
                UsuarioId = billetera.UsuarioId,
                SaldoTotal = billetera.SaldoTotal,
                SaldoRetenido = billetera.SaldoRetenido,
                SaldoDisponible = billetera.SaldoDisponible
            };
            return Ok(result);
        }

        // DTO exclusivo para la operación de depósito
        public record DepositoDto(int UsuarioId, decimal Monto);

        [HttpPost("deposito")]
        public async Task<ActionResult<BilleteraDto>> DepositoFondos([FromBody] DepositoDto dto)
        {
            if (dto.Monto <= 0)
                return BadRequest("El monto a depositar debe ser mayor a cero.");

            // 1. Obtener la entidad real desde la base de datos
            var billetera = await _repo.GetByUsuarioIdAsync(dto.UsuarioId);
            if (billetera == null)
                return NotFound("Billetera no encontrada.");

            // 2. Aplicar la regla de negocio (sumar el monto)
            billetera.Depositar(dto.Monto); // Lógica encapsulada dentro de la Entidad Billetera

            // 3. Guardar cambios en el repositorio
            await _repo.UpdateAsync(billetera);

            // 4. Retornar 200 OK con el DTO actualizado
            return Ok(MapToDto(billetera));
        }


        private static BilleteraDto MapToDto(Billetera billetera)
        {
            return new BilleteraDto
            {
                Id = billetera.Id,
                UsuarioId = billetera.UsuarioId,
                SaldoTotal = billetera.SaldoTotal,
                SaldoRetenido = billetera.SaldoRetenido,
                SaldoDisponible = billetera.SaldoDisponible,
                Version = billetera.Version 
            };
        }
    }
}
