using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IAuditoriaRepository
    {
        Task AddAsync(AuditoriaLog auditoria);

        Task<IEnumerable<AuditoriaLog>> ObtenerPorEntidadAsync(string entidad, int entidadId);

        Task<IEnumerable<AuditoriaLog>> ObtenerTodosAsync();
    }
}
