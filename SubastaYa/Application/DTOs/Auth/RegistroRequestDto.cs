using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Auth
{
    public class RegistroRequestDto
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int Rol { get; set; }
    }
}
