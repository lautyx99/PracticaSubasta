using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public RolUsuario Rol { get; set; } 
        public DateTime FechaRegistro { get; set; }
    }
}
