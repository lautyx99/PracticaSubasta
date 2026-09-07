using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class AuditoriaLog
    {
        public int Id { get; private set; }

        public int UsuarioId { get; private set; }

        public string Entidad { get; private set; } = null!;

        public int EntidadId { get; private set; }

        public string Accion { get; private set; } = null!;

        public string DetalleJson { get; private set; } = null!;

        public DateTime Fecha { get; private set; }

        public string Servicio { get; private set; } = null!;

        public virtual Usuario Usuario { get; private set; } = null!;

        private AuditoriaLog() { }

        public AuditoriaLog(int usuarioId, string entidad, int entidadId, string accion, string detalleJson, DateTime fecha, string servicio)
        {
            UsuarioId = usuarioId;
            Entidad = entidad;
            EntidadId = entidadId;
            Accion = accion;
            DetalleJson = detalleJson;
            Fecha = fecha;
            Servicio = servicio;
        }

    }
}
