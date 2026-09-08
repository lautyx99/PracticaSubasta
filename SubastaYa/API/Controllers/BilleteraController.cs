using Application.DTOs.Billetera;
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
    }
}
