using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class AuditoriaLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Entidad { get; set; }

        public int EntidadId { get; set; }

        public string Accion { get; set; }

        public string DetalleJson { get; set; }

        public DateTime Fecha { get; set; }

        public string Servicio { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

    }
}
