using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; } 

        public string Nombre { get; set; }

        public string ContraseñaHash { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Rol { get; set; }
    }
}
