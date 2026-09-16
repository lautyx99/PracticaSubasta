using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Administrador")] // Solo accesible por Admins
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
        public async Task<IActionResult> ObtenerLogs([FromQuery] string entidad, [FromQuery] int entidadId)
        {
            var logs = await _auditoriaRepository.ObtenerPorEntidadAsync(entidad, entidadId);
            return Ok(logs);
        }
    }
}
