using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAuditoriaService
    {
        Task RegistrarEventoAsync(int? usuarioId, string entidad, int entidadId, string accion, object detalles, string servicio);
    }

}
