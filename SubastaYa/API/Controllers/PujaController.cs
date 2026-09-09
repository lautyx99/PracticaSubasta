using Application.DTOs.Puja;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PujaController : ControllerBase
    {
        private readonly IPujaRepository _repo;

        public PujaController(IPujaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PujaDto>>> GetBySubasta(int subastaId)
        {
            var pujas = await _repo.GetBySubastaIdAsync(subastaId);

            var result = pujas.Select(p => new PujaDto
            {
                Id = p.Id,
                SubastaId = p.SubastaId,
                CompradorId = p.CompradorId,
                Monto = p.Monto,
                FechaPuja = p.Fecha_Puja
            });
            return Ok(result);
        }
    }
}
