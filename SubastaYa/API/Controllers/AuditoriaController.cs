using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "3")] // Solo accesible por Admins
    [ApiController]
    [Route("api/auditoria")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaController(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerLogs([FromQuery] string? entidad, [FromQuery] int? entidadId)
        {
            // Si el frontend no envía filtros, devolvemos todo el historial
            if (string.IsNullOrEmpty(entidad) || !entidadId.HasValue)
            {
                var todosLosLogs = await _auditoriaRepository.ObtenerTodosAsync(); // O el método equivalente en tu repo
                return Ok(todosLosLogs);
            }

            var logs = await _auditoriaRepository.ObtenerPorEntidadAsync(entidad, entidadId.Value);
            return Ok(logs);
        }
    }
}
