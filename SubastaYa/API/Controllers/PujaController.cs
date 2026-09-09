using Application.DTOs.Puja;
using Application.UseCases.Pujas;
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
        private readonly ObtenerPuja obtenerPuja;

        public PujaController(ObtenerPuja obtenerPuja)
        {
            this.obtenerPuja = obtenerPuja;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PujaDto>>> GetBySubasta(int subastaId)
        {
            var pujas = await obtenerPuja.ExecuteAsync(subastaId);

            return Ok(pujas);
        }
    }
}
